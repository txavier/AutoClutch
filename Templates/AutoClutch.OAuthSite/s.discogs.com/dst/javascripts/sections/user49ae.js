ds.define('user', function(require) {
  var $, UserHeader, alerts, events, features, getText, getUrl, i18n, layout, nameCheckState, pub, ready, renderHeardFromClass, trackEvent, util, _, _ref;
  _ref = require('$', 'layout', 'ready', 'alerts', '_', 'features', 'util', 'i18n'), $ = _ref.$, layout = _ref.layout, ready = _ref.ready, alerts = _ref.alerts, _ = _ref._, features = _ref.features, util = _ref.util, i18n = _ref.i18n;
  getText = i18n.getText;
  events = require('events');
  getUrl = util.getUrl;
  i18n.load('sections/user');
  pub = {};
  pub.init = function() {
    var fn;
    events.on('user:namecheck:loading user:namecheck:finished', pub.renderNameCheck);
    if (util.deparam().username) {
      pub.checkName();
    }
    fn = _.throttle(pub.checkName, 1000, true);
    $("#username").on("keyup blur", fn);
    $("#heard_from").on("change", pub.selectHeardFrom);
    return this.view - new UserHeader();
  };
  renderHeardFromClass = function(value) {
    return "heard_from_" + (value.replace(" ", "_").toLowerCase());
  };
  pub.selectHeardFrom = function(event) {
    var nameOfSelection, otherClasses;
    nameOfSelection = renderHeardFromClass(event.target.value);
    otherClasses = $("#heard_from option").map(function() {
      return renderHeardFromClass(this.value);
    }).toArray().join(" ");
    return $("#heard_from_field").removeClass(otherClasses).addClass(nameOfSelection);
  };
  trackEvent = function(type, action) {
    type = type.charAt(0).toUpperCase() + type.slice(1);
    return util.track('Marketplace', "" + type + " Banner " + action);
  };
  UserHeader = Backbone.View.extend({
    el: "#user_header",
    initialize: function() {
      this.baseUrl = getUrl("/user/banner_images");
      this.type = this.$el.find("form").data("type");
      if (this.type === "user" && layout.mode() === "mobile") {
        return this.showSelection();
      }
    },
    events: function() {
      return {
        "change #header_image_file": features.hasFormData ? this.userHeaderImageUpload : this.ieUpload,
        "click #enable_header_image": this.enableUserHeaderImage,
        "click #disable_header_image": this.disableUserHeaderImage,
        "click #delete_header_image": this.removeUserHeaderImage,
        "click .image_upload_icon": this.showUserHeaderControls
      };
    },
    showSelection: function() {
      var $activeSection, position, screenWidth;
      $activeSection = $(".active_profile_section")[0];
      if (!$activeSection) {
        return;
      }
      position = $activeSection.offsetLeft + $activeSection.clientWidth;
      screenWidth = $(document).width();
      if (position > screenWidth) {
        return $(".user_profile_wrap").scrollLeft(position - screenWidth);
      }
    },
    reportErrors: function(errors) {
      var errorMessage;
      this.$el.removeClass("image_uploading");
      errorMessage = errors ? errors.join("\n\n") : getText("An error has occurred, please try again.")();
      return alerts.alert(errorMessage);
    },
    previewImage: function(data) {
      var imageUrl, _ref1, _ref2, _ref3;
      imageUrl = layout.mode() === "mobile" ? (_ref1 = data.image) != null ? _ref1.url_mobile : void 0 : (_ref2 = data.image) != null ? _ref2.url : void 0;
      if (imageUrl) {
        $("#user_header").css("background-image", "url('" + imageUrl + "')");
      }
      this.$el.addClass('image_uploaded');
      this.$el.removeClass('image_enabled');
      trackEvent(this.type, "Uploaded");
      return this.setBannerId(this.$el, (_ref3 = data.image) != null ? _ref3.id : void 0);
    },
    ajaxOptions: function(url, imageData) {
      return {
        url: url,
        type: "POST",
        data: imageData ? imageData : void 0,
        cache: false,
        dataType: "json",
        processData: false,
        contentType: false
      };
    },
    showUserHeaderControls: function(e) {
      var $controls;
      $controls = this.$el.find(".header_controls");
      $controls.toggleClass("show_header_controls");
      $('.image_upload_icon').show();
      return $(e.currentTarget).hide();
    },
    userHeaderImageUpload: function(e) {
      var action, imageData, url, userImage;
      imageData = new FormData();
      userImage = e.target.files[0];
      imageData.append("image", userImage);
      if (userImage) {
        this.$el.addClass("image_uploading");
      }
      url = "" + this.baseUrl + "/upload/" + this.type;
      action = $.ajax(this.ajaxOptions(url, imageData));
      action.done((function(_this) {
        return function(data) {
          _this.$el.removeClass("image_uploading");
          return _this.previewImage(data);
        };
      })(this));
      return action.fail((function(_this) {
        return function(xhr) {
          var data;
          data = xhr.responseJSON;
          return _this.reportErrors(data.errors);
        };
      })(this));
    },
    getBannerId: function($el) {
      return $el.data('bannerId');
    },
    setBannerId: function($el, id) {
      return $el.data('bannerId', id);
    },
    enableUserHeaderImage: function(e) {
      var action, url;
      e.preventDefault();
      url = "" + this.baseUrl + "/" + (this.getBannerId(this.$el)) + "/enable";
      trackEvent(this.type, "Enabled");
      action = $.ajax(this.ajaxOptions(url));
      action.done((function(_this) {
        return function() {
          return _this.$el.addClass('image_enabled');
        };
      })(this));
      return action.fail((function(_this) {
        return function(xhr) {
          var data;
          data = xhr.responseJSON;
          return _this.reportErrors(data.errors);
        };
      })(this));
    },
    disableUserHeaderImage: function(e) {
      var action, url;
      e.preventDefault();
      url = "" + this.baseUrl + "/" + (this.getBannerId(this.$el)) + "/disable";
      trackEvent(this.type, "Disabled");
      action = $.ajax(this.ajaxOptions(url));
      action.done((function(_this) {
        return function() {
          return _this.$el.removeClass('image_enabled');
        };
      })(this));
      return action.fail((function(_this) {
        return function(xhr) {
          var data;
          data = xhr.responseJSON;
          return _this.reportErrors(data.errors);
        };
      })(this));
    },
    removeUserHeaderImage: function(e) {
      var action, url;
      e.preventDefault();
      url = "" + this.baseUrl + "/" + (this.getBannerId(this.$el)) + "/delete";
      action = $.ajax(this.ajaxOptions(url));
      trackEvent(this.type, "Removed");
      action.done((function(_this) {
        return function() {
          _this.$el.removeClass('image_uploaded');
          _this.$el.removeClass('image_enabled');
          return $("#user_header").css("background-image", "none");
        };
      })(this));
      return action.fail((function(_this) {
        return function(xhr) {
          var data;
          data = xhr.responseJSON;
          return _this.reportErrors(data.errors);
        };
      })(this));
    },
    ieUpload: function() {
      return this.$el.find('#user_header_form').ajaxSubmit({
        url: "" + this.baseUrl + "/upload/" + this.type,
        dataType: "text",
        success: (function(_this) {
          return function(data) {
            var response;
            response = $.parseJSON(data);
            try {
              if (response.image) {
                return _this.previewImage(response);
              } else {
                throw "error";
              }
            } catch (_error) {
              return _this.reportErrors(response.errors);
            }
          };
        })(this)
      });
    }
  });
  pub.setIsLoading = function() {
    return events.trigger('user:namecheck:loading', {
      isLoading: true,
      message: "<i class='icon icon-refresh'></i> " + (getText('Checking')()) + "..."
    });
  };
  nameCheckState = {
    isLoading: false,
    isAvailable: null,
    message: ''
  };
  pub.checkName = function() {
    var username;
    username = $("#username").val();
    if (pub.lastCheckedUsername !== username) {
      pub.lastCheckedUsername = username;
      $.getJSON(getUrl("/users/namecheck"), {
        username: username
      }).then(_.partial(pub.nameCheckFinish, true), _.partial(pub.nameCheckFinish, false));
      return pub.setIsLoading();
    }
  };
  pub.nameCheckFinish = function(isAvailable, response) {
    return events.trigger('user:namecheck:finished', {
      message: response.message || response.responseJSON.errors,
      isAvailable: isAvailable,
      isLoading: false
    });
  };
  pub.renderNameCheck = function(state) {
    var isAvailable, isLoading, message, _ref1;
    _ref1 = _.defaults(state, nameCheckState), isAvailable = _ref1.isAvailable, isLoading = _ref1.isLoading, message = _ref1.message;
    return $("#namecheckmsg").show().toggleClass("avail", !isLoading && isAvailable).toggleClass("not_avail", !isLoading && !isAvailable).html(message);
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
