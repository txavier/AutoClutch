ds.define('artist', function(require) {
  var $, alerts, card, dsdata, features, getRowHTML, getText, getUrl, i18n, menu, models, onNextVisit, pub, ready, util, _, _ref;
  _ref = require('$', '_', 'dsdata', 'features', 'menu', 'util', 'alerts', 'onNextVisit', 'ready', 'i18n'), $ = _ref.$, _ = _ref._, dsdata = _ref.dsdata, features = _ref.features, menu = _ref.menu, util = _ref.util, alerts = _ref.alerts, onNextVisit = _ref.onNextVisit, ready = _ref.ready, i18n = _ref.i18n;
  getUrl = util.getUrl;
  getText = i18n.getText;
  models = require('models');
  card = require('card');
  pub = {};
  pub.init = function() {
    var $body, _ref1;
    if ($.pjax.defaults) {
      $.pjax.defaults.scrollTo = (_ref1 = $("#discography_wrapper").offset()) != null ? _ref1.top : void 0;
    }
    this.artistID = dsdata["artist/view:artistID"];
    if (!this.artistID) {
      return;
    }
    this.defaultCredit = dsdata["artist/view:defaultCredit"].split("_");
    $body = $("body");
    $body.on("click", ".artist_track_link", this.trackLink).on("click", ".edit_master", this.masterEdit).on("click", ".credit_type", this.creditFilter).on("submit", ".artist_search_form", this.submitSearch).on("click", "#mr_a_create", this.createMaster);
    $(window).on("pjax:start", this.pjaxStart).on("pjax:end", this.pjaxEnd);
    $(window).on("cards:initialized", (function(_this) {
      return function(e, id) {
        if ($("#mr_panel").is(":visible")) {
          return _this.setupSubCheckboxes(id);
        }
      };
    })(this));
    if (features.isMobile) {
      return menu.setupActionMenuView($body);
    }
  };
  pub.pjaxStart = function() {
    var $creditType, filter, queryString, subtype;
    $creditType = $(".credit_type");
    queryString = util.deparam();
    subtype = queryString.subtype || "All";
    if (queryString.recent) {
      filter = ".recent";
    } else {
      filter = "[data-credit-type=\"" + queryString.type + "\"][data-credit-subtype=\"" + subtype + "\"]";
    }
    if (!queryString.type && !queryString.recent) {
      $creditType.filter(".selected").removeClass("selected");
      if (!queryString.query) {
        $creditType.filter(".default").addClass("selected");
      }
      return;
    }
    $creditType.filter(".selected").removeClass("selected");
    $creditType.filter(filter).addClass("selected");
  };
  pub.pjaxEnd = function() {
    if ($("#mr_panel").is(":visible")) {
      pub.createMasterCheckboxes();
      pub.checkSelectedItems();
    }
    $(".discography_nav li").find(".icon-spinner").remove();
    $(".artist_search_form .icon-spinner").removeClass("icon-spinner icon-spin").addClass("icon-search");
    if (features.isMobile) {
      $("#discography_wrapper").removeClass("open");
    }
  };
  pub.isEmpty = function() {
    var htmlIdData;
    htmlIdData = $.trim(this.$discography.html());
    return _.isBlank(htmlIdData);
  };
  pub.hideChildren = function() {
    $(".default_releases, .discography_wrapper, .search_results").hide();
  };
  pub.setupSubCheckboxes = function(id) {
    var model, objectId, view;
    if (id || pub.isEditing) {
      objectId = id ? id : parseInt(pub.isEditing.id);
      model = models.cards.where({
        objectType: 'master release',
        objectId: objectId
      })[0];
      view = _.where(card.views, {
        model: model
      })[0];
      if (view) {
        return view.toggleVersions(true).then(function() {
          return pub.createMasterCheckboxes();
        });
      }
    }
  };
  pub.checkSelectedItems = function() {
    var _ref1;
    if ((_ref1 = pub.isEditing) != null ? _ref1.versions : void 0) {
      return _.each(pub.isEditing.versions, function(version) {
        return $(".r" + version.id).prop("checked", true);
      });
    }
  };
  pub.reset = function(id) {
    var resetID;
    if (id) {
      resetID = $("#" + id);
      resetID.html("\n");
    } else {
      $("#releases").children().html("\n");
    }
  };
  pub.trackLink = function() {
    util.track("artist", $(this).data("action"));
  };
  pub.masterEdit = function(e) {
    var id, parentRow;
    if (features.isMobile) {
      return;
    }
    parentRow = $(this).parents("tr");
    id = pub.getObid(parentRow.get()[0]);
    if (id.charAt(0) === "m") {
      pub.clickMasterEdit(null, id);
    }
    e.preventDefault();
  };
  pub.clickMasterEdit = function(e, id) {
    id = id.substr(1);
    if (util.requireLogin()) {
      $("button.button_mr").prop("disabled", true);
      $.ajax({
        url: getUrl("/master/data"),
        type: "GET",
        data: {
          master_id: id
        },
        success: pub.startEditMaster,
        error: function(xhr) {
          alerts.alert(util.errorMessageFromXHR(xhr));
        }
      });
    }
  };
  pub.startEditMaster = function(data) {
    var $els, $saveButton;
    pub.isEditing = data;
    pub.isEditing.originalIds = _.keys(data.versions);
    $saveButton = $("#mr_save_button");
    $els = void 0;
    pub.setupSubCheckboxes(parseInt(data.id, 10));
    pub.createMaster(null, data.id);
    $("#mr_notes").val(data.notes);
    pub.updateMaster();
  };
  pub.submitSearch = function(e) {
    var $pjaxContainer, $target, query, url;
    e.preventDefault();
    e.stopPropagation();
    $target = $(e.target);
    query = $target.find(".artist_search_query").val();
    url = window.location.pathname + "?query=" + query;
    if ($.support.pjax) {
      $pjaxContainer = $("#pjax_container");
      $target.find(".icon-search").removeClass("icon-search").addClass("icon-spinner icon-spin");
      $(".credit_type.selected").removeClass("selected");
      $.pjax({
        url: url,
        container: $pjaxContainer,
        timeout: $pjaxContainer.data("pjax-timeout")
      });
    } else {
      window.location.href = url;
    }
  };
  pub.creditFilter = function(e) {
    var $credit, $pjaxContainer;
    $credit = $(this);
    if ($credit.is(".selected")) {
      return false;
    }
    $pjaxContainer = $("#pjax_container");
    $credit.append("<i class=\"icon icon-spinner icon-spin\" />");
    $("#search_query").val("");
    pub.updateDocumentTitleFromCredit($credit);
    $.pjax.click(e, {
      container: $pjaxContainer,
      timeout: $pjaxContainer.data("pjax-timeout")
    });
  };
  pub.updateDocumentTitleFromCredit = function($credit) {
    return document.title = $credit.attr('data-credit-document-title');
  };
  pub.getObid = function(el) {
    var clas;
    clas = $(el).attr("class");
    return clas.match(/[a-z][0-9]+/)[0];
  };
  pub.createMaster = function(e, id) {
    var $body, $masterPanel, $panelTitle, $saveButton, marginBottom;
    if (features.isMobile) {
      return;
    }
    $masterPanel = $("#mr_panel");
    marginBottom = 0;
    $body = $("body");
    $saveButton = $("#mr_save_button");
    $panelTitle = $("#mr_panel_title");
    if ($masterPanel.length < 1) {
      return;
    }
    pub.createMasterCheckboxes();
    i18n.load('sections/artist').then(function() {
      $saveButton.html(id ? getText("Save") : getText("Create")()).off("click");
      if (id) {
        $saveButton.on("click", {
          id: id
        }, pub.saveMaster);
      } else {
        $saveButton.click(pub.saveMaster);
      }
      $("#mr_cancel_button").click(pub.cancelMaster);
      $masterPanel.show();
      return $panelTitle.html(id ? getText("Edit Master Release")() : getText("Create Master Release")());
    });
    marginBottom = $body.css("margin-bottom").match(/\d+/);
    marginBottom = parseInt(marginBottom, 10) || 0;
    marginBottom += 212;
    $body.css("margin-bottom", marginBottom);
    if (e) {
      e.preventDefault();
    }
  };
  pub.createMasterCheckboxes = function() {
    var $els;
    $els = $("span.r_check");
    $els.each(function() {
      var $input, $self, obid, _ref1;
      $self = $(this);
      obid = pub.getObid($self.parents("tr"));
      if (obid.charAt(0) !== "r") {
        return;
      }
      if ($(this).find('input').length < 1) {
        $input = $("<input type=\"checkbox\" class=\"r_check " + obid + "\" />");
        $input.prop('checked', !!((_ref1 = pub.isEditing) != null ? _ref1.versions[obid.substr(1)] : void 0));
        $self.append($input);
      }
    });
    $("input.r_check:checkbox").off("click").on("click", pub.rcheckMaster);
  };
  pub.enableButtons = function() {
    $("#mr_save_button").prop("disabled", false);
    $("#mr_cancel_button").prop("disabled", false);
  };
  pub.disableButtons = function() {
    $("#mr_save_button").prop("disabled", true);
    $("#mr_cancel_button").prop("disabled", true);
  };
  pub.rcheckMaster = function() {
    var $el, $els, id;
    $el = $(this);
    $els = $("input." + this.className);
    id = parseInt(pub.getObid($el).slice(1), 10);
    $els.each(function() {
      var $self;
      $self = $(this);
      if ($self[0] !== $el[0]) {
        $self.prop("checked", $el.is(":checked"));
      }
    });
    if (!pub.isEditing) {
      pub.isEditing = {
        versions: {},
        main: id
      };
    }
    if ($el.is(":checked")) {
      pub.isEditing.versions[id] = {
        id: id,
        html: getRowHTML(id)
      };
    } else {
      delete pub.isEditing.versions[id];
    }
    pub.updateMaster();
  };
  pub.updateMaster = function() {
    var $panel, html;
    $panel = $("#mr_panel_inner");
    pub.override = false;
    html = require('templates').editMasterTable({
      versions: pub.isEditing.versions
    });
    $panel.html(html);
    $(".r_radio.r" + pub.isEditing.main).prop("checked", true);
  };
  pub.saveMaster = function(e) {
    var $radios, addIds, id, ids, keyRelease, mode, notes, params, removeIds, updatedIds;
    $radios = $("input.r_radio");
    id = e.data && e.data.id;
    ids = [];
    updatedIds = [];
    keyRelease = false;
    params = {
      data: {}
    };
    removeIds = void 0;
    if (!$radios.length) {
      return;
    }
    if ($radios.filter(":checked").length > 0) {
      keyRelease = pub.getObid($radios.filter(":checked")).substr(1);
    }
    ids = pub.isEditing.originalIds;
    updatedIds = _.keys(pub.isEditing.versions);
    addIds = _(updatedIds).difference(ids);
    removeIds = _(ids).difference(updatedIds);
    if (!keyRelease) {
      alerts.alert(getText("Please select a key release.")());
      return;
    }
    params.data["sub_notes"] = $("#sub_notes").val();
    if (pub.override) {
      params.data.override = 1;
    }
    notes = $("#mr_notes").val();
    if (id) {
      if (notes !== encodeURIComponent(pub.isEditing.notes)) {
        params.data["mr_notes"] = notes;
      }
      if (keyRelease !== pub.isEditing.main) {
        params.data.main = keyRelease;
      }
      if (removeIds.length) {
        params.data.remove = removeIds.join(",");
      }
      if (addIds.length) {
        params.data.add = addIds.join(",");
      }
      params.data["master_id"] = pub.isEditing.id;
      params.url = getUrl("/master/save");
      mode = "edited";
    } else {
      params.data.ids = _.keys(pub.isEditing.versions).join(",");
      params.data.main = keyRelease;
      params.data["mr_notes"] = notes;
      params.url = getUrl("/master/new");
      mode = "created";
    }
    params.success = function(data) {
      if (data.type === "success") {
        pub.saveMasterSuccess();
      } else if (data.type === "warn") {
        pub.saveMasterWarn(data.msg);
      } else {
        pub.saveMasterError(data.msg);
      }
      onNextVisit("util.track", "master release", mode, "on artist page", util.getDuration());
    };
    params.error = function(xhr) {
      pub.saveMasterError(util.errorMessageFromXHR(xhr));
    };
    params.type = "POST";
    pub.disableButtons();
    $.ajax(params);
  };
  pub.saveMasterSuccess = function() {
    pub.cancelMaster();
    alerts.alert(getText("Master release saved successfully and will be processed soon. Please refresh the page in a minute to see the updated discography.")());
  };
  pub.saveMasterError = function(msg) {
    pub.enableButtons();
    alerts.alert(getText("Error")() + ":\n\n" + msg);
  };
  pub.saveMasterWarn = function(msg) {
    var description, saveButton, warning;
    saveButton = $("#mr_save_button").html();
    warning = getText("Warning")();
    description = getText("Click %(saveButton)s again to save anyway.")({
      saveButton: saveButton
    });
    alerts.alert("" + warning + ":\n\n" + msg + "\n\n" + description);
    pub.enableButtons();
    pub.override = 1;
  };
  pub.cancelMaster = function() {
    var $els;
    $els = $("span.r_check");
    $els.html("");
    $("#mr_panel").hide();
    $("#mr_panel_inner").html("");
    $("#mr_notes").val("");
    $("#sub_notes").val("");
    pub.enableButtons();
    $("body").css("margin-bottom", 0);
  };
  getRowHTML = function(id) {
    var $row, classes, getHTML;
    getHTML = function(i, element) {
      return element.outerHTML;
    };
    $row = $("#r" + id);
    classes = ".image, .title, .label, .year";
    return $row.find(classes).map(getHTML).get().join("");
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
