ds.define('release', function(require) {
  var $, card, dsdata, menu, models, pub, ready, skittles, util, _, _ref;
  _ref = require('$', '_', 'dsdata', 'menu', 'card', 'skittles', 'util', 'ready'), $ = _ref.$, _ = _ref._, dsdata = _ref.dsdata, menu = _ref.menu, card = _ref.card, skittles = _ref.skittles, util = _ref.util, ready = _ref.ready;
  require('vimShortcuts');
  models = require('models');
  pub = {};
  pub.master = {};
  pub.init = function() {
    var $buttons, queryString;
    this.releaseID = dsdata["release/view:releaseId"];
    this.override = false;
    this.pageObj = dsdata.pageObject;
    this.model = models.cards.get(this.pageObj.id);
    if (this.model) {
      this.$partials = $(".card_model_partial");
      this.render = _.partial(card.renderCardModelState, this.$partials);
      this.model.on("change:wantlist change:collection", this.render);
      models.wantlist.on("add remove", this.renderBlocks);
      models.collection.on("add remove", this.renderBlocks);
      models.wantlist.on("add remove", this.renderStats);
      models.collection.on("add remove", this.renderStats);
      $buttons = $(".release_collections_menu");
      $buttons.find(".toggle_wantlist").onFastClick(pub.toggleWantlist);
      $buttons.find(".add_to_collection").onFastClick(pub.addToCollection);
      $buttons.find(".remove_from_collection").onFastClick(pub.removeFromCollection);
      $buttons.find(".want_add_all_button").onFastClick(pub.toggleAllWantList);
    }
    queryString = util.deparam();
    if (queryString["add_to_wantlist"]) {
      pub.addToWantlist();
    }
    return menu.setupActionMenuView($(".body"));
  };
  pub.renderStats = function(model, collection) {
    var $num, isAdding, isWantlist, numToAdd;
    isWantlist = model instanceof models.WantlistModel;
    isAdding = collection.include(model);
    $num = $(isWantlist ? ".want_num" : ".coll_num");
    numToAdd = isAdding && collection.length === 1 ? 1 : (!collection.length ? -1 : 0);
    $num.html(Number($num.html()) + numToAdd);
  };
  pub.renderBlocks = function(model, collection) {
    var $block, $wantlistBlock, blockType, isAdding, isWantlist, releaseID;
    isWantlist = model instanceof models.WantlistModel;
    blockType = isWantlist ? "wantlist" : "collection";
    isAdding = collection.include(model);
    releaseID = model.get(isWantlist ? "id" : "release_id");
    if (releaseID !== pub.releaseID) {
      return;
    }
    if (!isAdding) {
      $(".cw_block_" + (isWantlist ? "wantlist" : "collection[data-id='" + (model.get("id")) + "']")).remove();
    } else {
      $block = $($.trim(_.template($("#newBlockTemplate").html())({
        isWantlist: isWantlist,
        blockType: blockType,
        releaseID: releaseID
      })));
      $wantlistBlock = $(".cw_block_wantlist");
      if ($wantlistBlock.length) {
        $wantlistBlock.before($block);
      } else {
        $(".collections .section_content").append($block);
      }
      require('notes').init();
      model.on("sync", function() {
        $block.attr("data-id", model.get("id"));
        $block.find('.notes_field').data("coll-id", model.get("id"));
        $block.find(".remove_from_collection").onFastClick(pub.removeFromCollection);
        return $block.find(".toggle_wantlist").onFastClick(pub.toggleWantlist);
      });
    }
  };
  pub.hideAddToListMenu = function() {
    pub.$utilShow.off("click.addToListMenu");
    pub.$utilShow.on("click.addToListMenu", pub.showAddToListMenu);
    $(window).off("click.addToListMenu");
    pub.$utilShow.removeClass("active");
    pub.$addToListMenu.removeClass("active");
  };
  pub.collectionUpdate = function(id, count) {
    $(".r_tr." + id).toggleClass("collection", count).each(function() {
      var $self;
      $self = $(this);
      skittles.updateStatus($self, {
        collection: count
      });
      return skittles.updateMasterRowSkittles($self);
    });
  };
  pub.wantlistUpdate = function(id, count) {
    $("tr.r_tr." + id).toggleClass("wantlist", count).each(function() {
      var $self;
      $self = $(this);
      skittles.updateStatus($self, {
        wantlist: count
      });
      return skittles.updateMasterRowSkittles($self);
    });
  };
  pub.toggleWantlist = function() {
    var inWantlist, method;
    inWantlist = pub.model.get("wantlist").length === 1;
    method = inWantlist ? "removeFrom" : "addTo";
    pub.model[method]("wantlist");
    return false;
  };
  pub.addToWantlist = function() {
    pub.model.addTo('wantlist');
    return false;
  };
  pub.addToCollection = function() {
    pub.model.addTo("collection");
    return false;
  };
  pub.removeFromCollection = function() {
    pub.model.removeFrom("collection", $(this).closest(".cw_block_collection").data("id"));
    return false;
  };
  pub.toggleAllWantList = function(e) {
    pub.model.toggleAllWantlist();
    e.preventDefault();
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
