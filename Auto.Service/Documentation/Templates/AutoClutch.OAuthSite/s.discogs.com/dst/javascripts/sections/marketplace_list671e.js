ds.define('marketplaceList', function(require) {
  var $, Backbone, communityExpand, layout, menu, pub, ready, util, _ref;
  _ref = require('$', 'menu', 'layout', 'util', 'Backbone', 'ready'), $ = _ref.$, menu = _ref.menu, layout = _ref.layout, util = _ref.util, Backbone = _ref.Backbone, ready = _ref.ready;
  pub = {};
  pub.init = function() {
    var $filtersContainer;
    $("body").on("click", "#toggle_tracklist", this.slideToggleTracklist);
    $filtersContainer = $('#marketplace_filters_container');
    if ($filtersContainer.length > 0) {
      pub.filterView = new pub.FilterView({
        el: $filtersContainer,
        model: new pub.FilterModel()
      });
      pub.filterRoutes = new pub.FilterRoutes();
      Backbone.history.start({
        root: location.pathname
      });
    }
    menu.setupActionMenuView($(".body"));
    $(document).on("pjax:end", pub.communityToggle);
    $('body').on({
      mouseenter: pub.communityToggle,
      mouseleave: pub.communityToggle,
      click: pub.communityToggle
    }, ".community_summary");
    return $(".cart_button").one('mousedown keydown', util.makeOnNextVisitHandler('util.track', 'item', 'Added to Cart'));
  };
  pub.FilterRoutes = Backbone.Router.extend({
    routes: {
      'more%3D:filter': 'showMore',
      '': 'showFilters'
    },
    showFilters: function() {
      return pub.filterView.hideMoreFiltersElement();
    },
    showMore: function(label) {
      return pub.filterView.trigger("showMoreFilters", label);
    }
  });
  pub.FilterModel = Backbone.Model.extend({});
  pub.FilterView = Backbone.View.extend({
    events: {
      "mouseover .show_more_filters": "loadMoreFilters",
      "click .hide_more_filters": "hideMoreFilters",
      "click .range_toggle": "rangeToggle",
      "click .show_more_filters": "moreFiltersClick"
    },
    initialize: function() {
      var view;
      view = this;
      this.allMarketplaceFilters = "";
      this.allLoadingFilterClasses = "";
      this.$el.find('.show_more_filters').each(function() {
        var endpoint, label;
        label = $(this).data("label");
        endpoint = $(this).data("endpoint");
        view.allMarketplaceFilters += " show_marketplace_filters_" + label;
        view.allLoadingFilterClasses += " loading_filters_" + label;
        return view.model.set(label, $.lazy$element(endpoint).appendTo("#more_filters_container"));
      });
      return this.on("showMoreFilters", this.showMoreFilters);
    },
    loadMoreFilters: function(e) {
      var label;
      label = $(e.target).closest('.show_more_filters').data("label");
      return this.model.get(label).call();
    },
    showMoreFilters: function(label) {
      this.$el.removeClass(this.allMarketplaceFilters);
      this.$el.addClass("show_marketplace_filters_" + label + " loading_filters_" + label);
      return this.model.get(label).call().then((function(_this) {
        return function() {
          if (_this.$el.hasClass("show_marketplace_filters_" + label)) {
            _this.$el.addClass("showing_more_filters");
          }
          _this.$el.removeClass("loading_filters_" + label);
          return _this.scrollToElement();
        };
      })(this));
    },
    hideMoreFilters: function(e) {
      e.preventDefault();
      if (layout.mode() === "desktop") {
        return pub.filterRoutes.navigate("//");
      } else {
        return this.hideMoreFiltersElement();
      }
    },
    moreFiltersClick: function(e) {
      var label;
      e.preventDefault();
      label = $(e.target).closest('.show_more_filters').data("label");
      if (layout.mode() === "desktop") {
        return pub.filterRoutes.navigate("more%3D" + label, {
          trigger: true
        });
      } else {
        return pub.filterRoutes.showMore(label);
      }
    },
    hideMoreFiltersElement: function() {
      return this.$el.removeClass("showing_more_filters " + this.allMarketplaceFilters);
    },
    scrollToElement: function() {
      var headerHeight;
      headerHeight = 130;
      if ($(document).scrollTop() > headerHeight) {
        return util.scrollTo(this.$el);
      }
    },
    rangeToggle: function(e) {
      var rangeSelector;
      rangeSelector = $(e.currentTarget).data("range");
      $(e.currentTarget).toggleClass('range_show');
      $(".filter_range_" + rangeSelector).toggle();
      e.preventDefault();
      return e.stopPropagation();
    }
  });
  pub.slideToggleTracklist = function() {
    $("#tracklist").slideToggle();
    util.track("Marketplace", "Clicked toggle tracklist");
  };
  communityExpand = false;
  pub.communityToggle = function(e) {
    var selectors;
    if (e.type === "click") {
      selectors = $('.community_summary').not(this);
      communityExpand = !communityExpand;
      return $(selectors).toggleClass('community_fixed_width');
    } else if (communityExpand && e.type === "pjax:end") {
      return $('.community_summary').toggleClass('community_fixed_width');
    } else if (!communityExpand) {
      return $(this).toggleClass('community_fixed_width');
    }
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
