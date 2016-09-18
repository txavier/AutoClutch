ds.define('recommendations', function(require) {
  var $, dsdata, features, getUrl, init, ready, _ref;
  _ref = require('$', 'features', 'dsdata', 'ready'), $ = _ref.$, features = _ref.features, dsdata = _ref.dsdata, ready = _ref.ready;
  getUrl = require('util').getUrl;
  init = function() {
    var $cards, $parent, $slider, cardWidth, pageNumber;
    $parent = $(".recommendations");
    if (!$parent.length) {
      return;
    }
    $slider = $parent.find("#recs_slider");
    $cards = $parent.find(".cards");
    cardWidth = $cards.find(".card_large").outerWidth(true);
    pageNumber = 1;
    $cards.css("width", $cards.data("num-recs") * cardWidth);
    return $slider.lazyGallery({
      has3d: features.has3d(),
      isMobile: features.isMobile,
      snapToSlide: false,
      slideContainerClass: ".cards",
      slideClass: ".card_large",
      slideCount: $cards.data("num-recs"),
      advance: 5,
      resize: false,
      get: function(pageNumber) {
        var args, sellerUid;
        args = {
          type: dsdata["recommendations/_macro:pagetype"],
          page: pageNumber
        };
        sellerUid = dsdata["recommendations/_macro:sellerid"];
        if (sellerUid) {
          args["seller"] = sellerUid;
        }
        $.get(getUrl("/release/recs/" + dsdata["recommendations/_macro:pageid"]), args, function(data) {
          return $cards.append(data);
        });
      }
    });
  };
  return ready(init);
});
