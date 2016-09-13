ds.define('marketplaceSellerList', function(require) {
  var $, alerts, features, getText, i18n, layout, makeReactDialog, pub, ready, showSellerShippingTerms, util, _, _ref;
  _ref = require('$', 'layout', 'ready', 'alerts', '_', 'features', 'util', 'i18n'), $ = _ref.$, layout = _ref.layout, ready = _ref.ready, alerts = _ref.alerts, _ = _ref._, features = _ref.features, util = _ref.util, i18n = _ref.i18n;
  getText = i18n.getText;
  makeReactDialog = require("reactDialogs").makeReactDialog;
  i18n.load('sections/marketplace_seller_list');
  pub = {};
  pub.init = function() {
    if (window.location.hash === "#terms") {
      showSellerShippingTerms();
    }
    $(".terms").click(showSellerShippingTerms);
    return false;
  };
  showSellerShippingTerms = function() {
    var $shippingDialog;
    $shippingDialog = $("#seller_shipping");
    return makeReactDialog({
      content: $shippingDialog.html(),
      title: getText("Shipping and Payment Terms")(),
      width: layout.mode() === "mobile" ? "100%" : 800,
      height: layout.mode() === "mobile" ? 400 : 600,
      open: function() {
        if (window.location.hash !== "#terms") {
          return window.location.hash = "#terms";
        }
      },
      close: function() {
        return window.location.hash = "";
      }
    });
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
