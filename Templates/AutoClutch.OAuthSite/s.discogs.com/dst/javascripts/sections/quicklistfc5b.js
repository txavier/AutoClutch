ds.define('quicklist', function(require) {
  var $, Promise, alerts, dsdata, formDisplayedTimestamp, getText, getUrl, i18n, imageGallery, images, inputCounter, load, makeReactDialog, marketplaceSellItem, priceSuggester, pub, ready, util, _, _ref;
  _ref = require('$', '_', 'util', 'dsdata', 'marketplaceSellItem'), $ = _ref.$, _ = _ref._, util = _ref.util, dsdata = _ref.dsdata, marketplaceSellItem = _ref.marketplaceSellItem;
  alerts = require('alerts');
  images = require('images');
  priceSuggester = require('priceSuggester');
  load = require('load');
  ready = require('ready');
  Promise = require('Promise');
  i18n = require('i18n');
  imageGallery = require('imageGallery');
  inputCounter = require('inputCounter');
  getText = i18n.getText;
  getUrl = util.getUrl;
  makeReactDialog = require("reactDialogs").makeReactDialog;
  i18n.load('sections/quicklist');
  pub = {};
  formDisplayedTimestamp = 0;
  pub.init = function() {
    var $body, $window;
    $body = $("body");
    $window = $(window);
    if (!$("#quicklister").length) {
      return;
    }
    this.slideOpen = "+=33%";
    this.slideClose = "-=33%";
    this.openTime = 250;
    this.closeTime = this.openTime / 2;
    this.searchable = ["artist", "title", "catno", "barcode", "label", "genre", "format", "year", "release_id"];
    this.defaultFilters = ["artist", "title"];
    this.apiRootUri = dsdata.apiServer;
    this.$resultsIndicator = $("#resultsIndicator");
    this.$postIndicator = $("#postIndicator");
    this.$resultsPane = $("#resultsPane");
    this.$postPane = $("#postPane");
    this.currency = dsdata["quicklist/index:currency"];
    this.sres = dsdata["quicklist:sres"];
    this.spost = dsdata["quicklist:spost"];
    this.currentFile = {};
    $("#acceptedFieldsHelp").on("click", this.clickAcceptedFieldsHelp);
    $("#qlsearch").on("submit", this.submitQlSearch);
    $("#qladvanced, .fileSearch").on("submit", this.submitFileOrAdvanced);
    $("#fileUpload").on("submit", this.submitFileUpload);
    $("#uploadTarget").on("load", this.loadUploadTarget);
    $("#uploads_open_file").on("click", this.clickSearchPaneLink);
    $("#startOver").on("click", this.clickStartOver);
    $body.on("click", ".fileUpload", this.clickFileUpload).on("click", ".nextPage", this.clickNextPage).on("click", "ul.uploadRows > li[class!=selected]", this.clickUploadRowItem).on("click", ".filterSearch", this.clickFilterSearch).on("submit", ".actionBar form", this.clickActionBarForm).on("click", "#salesHistoryDetails", this.clickSalesHistoryDetails).on("click", ".getMpItems", this.clickGetMpItems).on("keyup", "#postItemForSale input[name=price]", this.keyupSaleItemPrice).on("submit", "#postItemForSale", this.submitSaleItem).on("click", ".uploadRows li.listedItem", this.clickUploadItem).on("click", "#logIn", this.clickLogIn).on("click", "#createAccount", this.clickCreateAccount).on("click", ".prepareListing", this.clickPrepareListing).on("mouseenter mouseleave", ".prepareListing", this.hoverPrepareListing).on("keyup", "#comments", pub.checkWords).on({
      mouseenter: this.hoverPreviewRelease,
      mouseleave: this.hoverOffPreviewRelease
    }, ".previewRelease").on({
      mouseenter: this.hoverReleasePreview,
      mouseleave: this.hoverOffReleasePreview
    }, ".releasePreview");
    $window.on("resize", this.resizeWindow);
    this.resizeWindow();
    return this.evaluateSearch();
  };
  pub.checkWords = function() {
    var bad, words;
    words = $("#comments").val();
    bad = marketplaceSellItem.detectBadWords(words);
    $("#comment_notice").toggleClass("hidden", !bad);
  };
  pub.loadPanes = function($form) {
    var getFormResults, hideResultsIndicator;
    hideResultsIndicator = function() {
      pub.$resultsIndicator.hide();
    };
    getFormResults = function() {
      return pub.getResults($form, function() {
        pub.openPane(pub.$resultsPane).then(hideResultsIndicator);
      });
    };
    pub.$resultsIndicator.show();
    $("#resultsPaneDefault, #postPaneDefault").remove();
    if (pub.$postPane.is(":visible")) {
      pub.closePane(pub.$postPane).then(function() {
        pub.closePane(pub.$resultsPane).then(getFormResults);
      });
    } else {
      if (pub.$resultsPane.is(":visible")) {
        pub.closePane(pub.$resultsPane).then(getFormResults);
      } else {
        getFormResults();
      }
    }
  };
  pub.getResults = function($form, callback) {
    var qsMasterId, qsReleaseId, queryString;
    queryString = util.deparam();
    qsReleaseId = queryString["release_id"] || "";
    qsMasterId = queryString["master_id"] || "";
    if ($form.attr("id") === "qladvanced" && ($("#release_id").val() === qsReleaseId || $("#master_id").val() === qsMasterId)) {
      pub.openTime = 0;
      pub.parseSearchResults(pub.sres, $form);
      util.track("quick_lister", "reveal_release", "id_" + (qsReleaseId || qsMasterId));
    } else {
      pub.openTime = 250;
      $.getJSON(pub.searchReleasesUri($form.serialize()), function(data) {
        pub.parseSearchResults(data.data, $form);
      });
    }
    callback();
  };
  pub.parseSearchResults = function(data, $form) {
    var response;
    response = data;
    response.formObj = $form.serializeArray();
    pub.$resultsPane.scrollTop(0).find(".inner_pane").html(_.template($("#resultsTemplate").html())(response));
    $.each(response.results, function(index, result) {
      result.thumb = result.thumb || util.getDefaultImage(result.type);
      $("#resultsPane .inner_pane > ul").append(_.template($("#" + result.type + "ResultTemplate").html())({
        result: result
      }));
      return $("#resultsPane .inner_pane > ul > li:even").addClass("even");
    });
    if (response.results.length === 1) {
      pub.$postIndicator.show();
      pub.getRelease(response.results[0].id, function() {
        pub.populatePostItemForm($form);
        pub.$postPane.show().animate({
          left: pub.slideOpen
        }, pub.openTime, function() {
          pub.$postIndicator.hide();
        });
      });
    }
  };
  pub.renderConditionDescription = function() {
    var text, value;
    value = $(this).val();
    text = marketplaceSellItem.getConditionDescription(value);
    $("#condition_description").html(text).toggleClass("hidden", _.isBlank(value));
  };
  pub.getRelease = function(releaseId, callback) {
    var getReleaseFinish, promise;
    getReleaseFinish = function(data) {
      var $fileSearchCurrent, $mediaCondition, $suggesterButton;
      $fileSearchCurrent = $(".fileSearchCurrent");
      pub.$postPane.find(".inner_pane").html(_.template($("#postTemplate").html())(data));
      $suggesterButton = pub.$postPane.find(".suggested_price");
      $mediaCondition = pub.$postPane.find("select[name=condition]");
      priceSuggester.create($suggesterButton, pub.$postPane.find("#price"), $mediaCondition, data["suggested_prices"]);
      $mediaCondition.on("change", pub.renderConditionDescription);
      $("#post_section_artist_title a, #post_section_label_title a", pub.$postPane).attr("target", "_blank");
      if ($fileSearchCurrent) {
        pub.populatePostItemForm($fileSearchCurrent);
      }
      images.fixOrientation(pub.$postPane);
      formDisplayedTimestamp = new Date();
      inputCounter.init();
    };
    promise = Promise.resolve((function() {
      pub.$postPane.attr("class", "pane post-release-" + releaseId);
      if (pub.sres && pub.spost && pub.spost.releaseId === releaseId) {
        return pub.spost;
      } else {
        pub.openTime = 250;
        return $.get(getUrl("/sell/get_release/" + releaseId), function(data) {
          data.uploadId = pub.currentFile.id;
          data.rowNum = pub.currentFile.rowNum;
        });
      }
    })());
    promise.then(getReleaseFinish);
    if (typeof callback !== "undefined") {
      promise.then(callback);
    }
    return promise;
  };
  pub.populatePostItemForm = function($form) {
    var $allowOffers, $comments, $externalId, $location, $mediaCondition, $price, $sleeveCondition, mediaConditionValue, sleeveConditionValue;
    $mediaCondition = $("#postItemForSale select[name=condition]");
    $sleeveCondition = $("#postItemForSale select[name=sleeve_condition]");
    $comments = $("#postItemForSale input[name=comments]");
    $location = $("#postItemForSale input[name=item_location]");
    $externalId = $("#postItemForSale input[name=external_id]");
    $price = $("#postItemForSale input[name=price]");
    $allowOffers = $("#postItemForSale input[name=allow_offers]");
    mediaConditionValue = $form.children("input[name=media_condition]").val();
    sleeveConditionValue = $form.children("input[name=sleeve_condition]").val();
    if (mediaConditionValue) {
      $mediaCondition.val(mediaConditionValue);
    }
    if (sleeveConditionValue) {
      $sleeveCondition.val(sleeveConditionValue);
    }
    $comments.val($form.children("input[name=comments]").val());
    $location.val($form.children("input[name=location]").val());
    $externalId.val($form.children("input[name=external_id]").val());
    $price.val($form.children("input[name=price]").val());
    if ($form.children("input[name=allow_offers]").val() === "Y") {
      $allowOffers.click();
    }
    $price.trigger("keyup");
  };
  pub.renderRows = function(data) {
    var $uploadRows, page, pages, perPage;
    $uploadRows = $("ul.uploadRows");
    page = parseInt(data.pagination.page, 10);
    pages = parseInt(data.pagination.pages, 10);
    perPage = parseInt(data.pagination["per_page"], 10);
    if (page === 1) {
      $uploadRows.children().remove();
    }
    _.each(data.rows, function(row) {
      return $uploadRows.append(_.template($("#rowTemplate").html())({
        row: row,
        searchable: pub.searchable,
        default_filters: pub.defaultFilters
      }));
    });
    if (page < pages) {
      $uploadRows.append("<li><a class='nextPage' data-page='" + (page + 1) + "' >" + (getText("Load next")()) + " " + perPage + "</a><i class='paginationIndicator icon icon-spinner icon-spin' style='display:none'></i></li>");
    }
  };
  pub.doFilterDisplay = function(obj) {
    var $searchDisplay, $searchInput, $self, fieldName, isChecked;
    $self = $(obj);
    fieldName = $self.data("field");
    $searchInput = $(".fileSearch input[name=" + fieldName + "]");
    $searchDisplay = $(".searchDisplay_" + fieldName);
    isChecked = $self.prop("checked");
    $searchInput.prop("disabled", !isChecked);
    $searchDisplay.toggleClass("quicklist_search_display_gray", !isChecked);
  };
  pub.submitQlSearch = function(e) {
    var q;
    q = $('input[name="ql"]').val();
    if (q.length < 2) {
      return false;
    }
    pub.loadPanes($(this));
    util.track("quick_lister", "basic_search");
    e.preventDefault();
  };
  pub.submitFileOrAdvanced = function(e) {
    var $self;
    $self = $(this);
    pub.loadPanes($self);
    $(".fileSearchCurrent").removeClass("fileSearchCurrent");
    $self.addClass("fileSearchCurrent");
    util.track("quick_lister", "detail_search");
    e.preventDefault();
  };
  pub.submitFileUpload = function() {
    var $self;
    $self = $(this);
    if ($self.find("input[type=file]").val()) {
      $self.find("input[type=submit]").toggle();
      $("#uploadFileIndicator").toggle();
    } else {
      alerts.alert(getText("Please choose a file to upload")());
      return false;
    }
  };
  pub.loadUploadTarget = function() {
    var $li, progress, uploadData, uploadProgress, uploadText;
    uploadText = this.contentDocument.body.textContent || this.contentDocument.body.innerText;
    uploadData = $.parseJSON(uploadText);
    uploadProgress = null;
    $li = $('<li class="body_row" />').insertAfter("#uploadFiles li.header");
    progress = function() {
      $.get(getUrl("/sell/ql_upload_progress"), {
        token: uploadData.token
      }, function(data) {
        $li.html("" + (getText("Processing")()) + ", " + (Math.floor(data.progress * 100)) + "%");
        if (data.progress === 1) {
          clearTimeout(uploadProgress);
          $li.html("Completed! <br /><a class='fileUpload' data-id='" + uploadData.id + "'>Open: " + uploadData.filename + "</a>");
          $("#uploadFile-" + uploadData.id).trigger("click");
        } else {
          uploadProgress = setTimeout(progress, 1000);
        }
      });
    };
    uploadProgress = setTimeout(progress, 1000);
    $("#uploadFileIndicator").toggle();
  };
  pub.clickSearchPaneLink = function() {
    $(this).fadeOut();
    $("#uploadedFile").fadeOut(function() {
      return $(".uploadWrapper").fadeIn();
    });
  };
  pub.clickFileUpload = function() {
    $(".uploadWrapper").fadeOut(function() {
      return $("#uploadedFile").fadeIn();
    });
    pub.currentFile.id = $(this).data("id");
    $.getJSON(getUrl("/sell/ql_upload_rows"), {
      id: pub.currentFile.id
    }, function(data) {
      var $indicator, rendered;
      rendered = _.template($("#uploadedFileTemplate").html())({
        header: data.header,
        searchable: pub.searchable,
        default_filters: pub.defaultFilters
      });
      $indicator = $("#uploadedFileIndicator");
      $indicator.toggle();
      $("#uploadedFile").html(rendered);
      $("#uploads_open_file").fadeIn();
      pub.renderRows(data);
      $("ul.uploadRows > li:first").trigger("click");
      $indicator.toggle();
      $(".filterSearch").each(function() {
        return pub.doFilterDisplay(this);
      });
    });
  };
  pub.clickNextPage = function() {
    var $self;
    $self = $(this);
    $(".paginationIndicator").toggle();
    $.getJSON(getUrl("/sell/ql_upload_rows"), {
      id: pub.currentFile.id,
      page: $self.data("page")
    }, function(data) {
      $self.parent().remove();
      pub.renderRows(data);
      $(".filterSearch").each(function() {
        return pub.doFilterDisplay(this);
      });
    });
  };
  pub.clickUploadRowItem = function() {
    var $extra, $form, $self;
    $self = $(this);
    $extra = $self.find("div.extra");
    $form = $self.find("form.fileSearch");
    pub.currentFile.rowNum = $self.data("row-num");
    $("ul.uploadRows > li[class=selected]").removeClass("selected").children("div.extra").slideUp();
    $self.addClass("selected");
    $extra.slideDown(function() {
      pub.loadPanes($form);
      $(".fileSearchCurrent").removeClass("fileSearchCurrent");
      $form.addClass("fileSearchCurrent");
    });
  };
  pub.clickFilterSearch = function() {
    var $checked, $current;
    $current = $(".fileSearchCurrent");
    $checked = $(".filterSearch:checked");
    pub.doFilterDisplay(this);
    pub.defaultFilters = _.map($checked, function(f) {
      return $(f).data("field");
    });
    if ($current.length && $checked.length) {
      pub.loadPanes($current);
    }
  };
  pub.clickActionBarForm = function(e) {
    var $selected;
    $selected = $(".uploadRows li.selected");
    $.post(getUrl("/sell/ql_no_match"), {
      upload_id: pub.currentFile.id,
      row_num: pub.currentFile.rowNum
    });
    $selected.fadeOut(function() {
      $selected.next().trigger("click");
      return $selected.remove();
    });
    e.preventDefault();
  };
  pub.clickPrepareListing = function() {
    var $self, close, getReleaseData, isVisible, needsData, open, releaseId;
    open = function() {
      return pub.openPane(pub.$postPane);
    };
    close = function() {
      return pub.closePane(pub.$postPane);
    };
    getReleaseData = function() {
      var promise;
      pub.$postIndicator.show();
      promise = pub.getRelease($self.data("release-id"));
      promise.then(function() {
        return pub.$postIndicator.hide();
      });
      return promise;
    };
    $self = $(this);
    releaseId = $self.data("release-id");
    $("#resultsPane li.selected").removeClass("selected");
    $self.addClass("selected");
    needsData = !pub.$postPane.hasClass("post-release-" + releaseId);
    isVisible = pub.$postPane.is(":visible");
    if (needsData) {
      if (isVisible) {
        close().then(function() {
          return getReleaseData().then(open);
        });
      } else {
        getReleaseData().then(open);
      }
    } else {
      if (!isVisible) {
        open();
      }
    }
  };
  pub.openPane = function($pane) {
    return Promise.resolve($pane.show().animate({
      left: pub.slideOpen
    }, {
      duration: pub.openTime
    }).promise());
  };
  pub.closePane = function($pane) {
    return Promise.resolve($pane.animate({
      left: pub.slideClose
    }, {
      duration: pub.closeTime
    }).promise()).then(function() {
      return $pane.hide();
    });
  };
  pub.hoverPrepareListing = function() {
    $(this).find("a.previewRelease").toggleClass("hidden");
  };
  pub.clickSalesHistoryDetails = function() {
    $("#historyDetails").slideToggle();
    util.track("quick_lister", "show_sales_history");
  };
  pub.clickGetMpItems = function() {
    var $self;
    $self = $(this);
    if ($self.hasClass("loaded")) {
      $("#mpItems").slideToggle();
      return;
    }
    $("#mpItems").slideToggle();
    $("#mpResultsIndicator").show();
    $.ajax({
      url: "" + pub.apiRootUri + "/marketplace/search",
      data: "release_id=" + ($self.data("release-id")),
      dataType: "jsonp",
      success: function(data) {
        $.each(data.data, function(index, result) {
          return $("#mpItems tbody").append(_.template($("#mpResultTemplate").html())({
            result: result
          }));
        });
        $self.addClass("loaded");
        $("#mpResultsIndicator").hide();
      }
    });
    util.track("quick_lister", "show_current_listings");
  };
  pub.keyupSaleItemPrice = function() {
    var price;
    price = parseFloat(this.value);
    if (isNaN(price) || typeof price !== "number" || price < 0.1) {
      $("#fee").html("0.00");
      return false;
    }
    $.ajax({
      url: "" + pub.apiRootUri + "/marketplace/fee/" + (price.toFixed(2)) + "/" + pub.currency,
      dataType: "jsonp",
      success: function(data) {
        $("#fee").html(data.data.value.toFixed(2));
      }
    });
  };
  pub.submitSaleItem = function(e) {
    var $selected, $self, dependencies;
    $self = $(this);
    $selected = void 0;
    dependencies = load("jquery.ui");
    $self.find("button[type=submit]").prop("disabled", true);
    $.ajax({
      type: "POST",
      url: getUrl("/sell/ql_post"),
      data: $self.serialize()
    }).done(function(data) {
      var $errors, duration;
      duration = void 0;
      if (data.errors.length) {
        $errors = $("#errors");
        $errors.show().html("<div>" + (getText("The following errors are preventing this item from being listed")()) + "</div><ul></ul>");
        $.each(data.errors, function(index, error) {
          return $errors.children("ul").append("<li>" + error + "</li>");
        });
        dependencies.then(function() {
          return $errors.effect("pulsate", {
            times: 2
          });
        });
        $self.find("button[type=submit]").prop("disabled", false);
      } else {
        $("#postSection").html(_.template($("#postSuccessTemplate").html())(data.item));
        $selected = $(".uploadRows li.selected");
        if ($selected.length) {
          $selected.html("" + pub.currentFile.rowNum + ". Listed for sale.").removeClass("selected").data("uri", getUrl("/sell/item/" + data.item["item_id"])).addClass("listedItem").next().trigger("click");
          $selected.highlight();
        }
        duration = Math.round((new Date() - formDisplayedTimestamp) / 1000);
        util.track("quick_lister", "post_item_done", undefined, duration);
        $("label[for=release_id], #release_id").remove();
        $("label[for=master_id], #master_id").remove();
      }
    });
    util.track("quick_lister", "post_item");
    e.preventDefault();
  };
  pub.clickUploadItem = function() {
    window.open($(this).data("uri"), "_blank");
  };
  pub.clickStartOver = function() {
    window.location = getUrl("/sell");
  };
  pub.clickLogIn = function() {
    window.location.href = getUrl("/login");
  };
  pub.clickCreateAccount = function() {
    window.location.href = getUrl("/users/create");
  };
  pub.clickAcceptedFieldsHelp = function(e) {
    makeReactDialog({
      content: $("#acceptedFieldsModal").html(),
      height: 560,
      width: 500,
      title: getText("Accepted Fields")()
    });
    e.preventDefault();
  };
  pub.hoverPreviewRelease = function() {
    var $preview, $self, offset, offsetLeft, offsetTop, t;
    $self = $(this);
    offset = $self.offset();
    $preview = $(".releasePreview");
    offsetLeft = offset.left - 633 > 0 ? offset.left - 633 : 0;
    offsetTop = offsetLeft === 0 ? offset.top + 20 : offset.top - 100;
    t = setTimeout(function() {
      return $preview.animate({
        top: offsetTop,
        left: offsetLeft
      }, 0, function() {
        var releaseId;
        releaseId = $self.parents("li.release").data("release-id");
        return $.get(getUrl("/sell/get_release_html/" + releaseId), {
          styleVersion: true
        }, function(data) {
          $preview.html(data).show().scrollTop(0);
          return imageGallery.initGallery($preview.find(".image_gallery"));
        });
      });
    }, 500);
    $self.data("timeout", t);
  };
  pub.hoverOffPreviewRelease = function() {
    var $releasePreview, t;
    $releasePreview = $(".releasePreview");
    t = void 0;
    clearTimeout($(this).data("timeout"));
    t = setTimeout(function() {
      return $releasePreview.hide();
    }, 250);
    $releasePreview.data("hideTimeout", t);
  };
  pub.hoverReleasePreview = function() {
    clearTimeout($(this).data("hideTimeout"));
  };
  pub.hoverOffReleasePreview = function() {
    var $self, t;
    $self = $(this);
    t = setTimeout(function() {
      return $self.hide();
    }, 250);
    $(this).data("hideTimeout", t);
  };
  pub.searchReleasesUri = function(params) {
    return pub.apiRootUri + ("/database/lookup?" + params + "&type=release&exclude=file&f=json&callback=?");
  };

  /*
  Search forms are auto-populated with values from query string
  Trigger a search if any are present
   */
  pub.evaluateSearch = function() {
    var $form, $searchInputs, searchInputHasValue;
    $searchInputs = $("#qlsearch input:text, #qladvanced input:text");
    searchInputHasValue = false;
    $searchInputs.each(function() {
      if ($(this).val()) {
        searchInputHasValue = true;
        return false;
      }
    });
    if (searchInputHasValue) {
      $form = $searchInputs.parents("form");
      pub.loadPanes($form);
      util.track("quick_lister", $form.data("ga"));
    } else {
      $("#resultsPaneDefault").show().animate({
        left: pub.slideOpen
      }, pub.openTime, function() {
        $("#postPaneDefault").show().animate({
          left: pub.slideOpen
        }, pub.openTime);
      });
    }
  };
  pub.resizeWindow = function() {
    var quicklistTop, windowHeight;
    windowHeight = $(window).height();
    quicklistTop = util.getElementMetrics($("#quicklister_interior")).top;
    $("#quicklister").find("#quicklister_interior, .pane, .pane .inner_pane, .my_upload_wrapper").height(Math.max(windowHeight - quicklistTop - 14 * 2, 560));
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
