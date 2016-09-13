var __modulo = function(a, b) { return (+a % (b = +b) + b) % b; };

ds.define('getStarted', function(require) {
  var $, events, features, getUrl, maxAPIPageLimit, pub, ready, util, youtubeIframeAPI, _, _ref;
  _ref = require('$', 'features', 'util', 'youtubeIframeAPI', '_', 'ready', 'events'), $ = _ref.$, features = _ref.features, util = _ref.util, youtubeIframeAPI = _ref.youtubeIframeAPI, _ = _ref._, ready = _ref.ready, events = _ref.events;
  getUrl = util.getUrl;
  maxAPIPageLimit = 50;
  pub = {
    init: function() {
      this.videoShowcase();
      this.community.init();
      $(".focus-header-search").on("click", this.focusSearch);
      if (!features.isMobile) {
        return $(window).on("scroll.getStarted", (function(_this) {
          return function(e) {
            return _this.scroller(e);
          };
        })(this)).on("load.getStarted", (function(_this) {
          return function() {
            setTimeout(_this.albumFlipper, 10000);
            $(_this).off("load.getStarted");
          };
        })(this));
      }
    },
    focusSearch: function(e) {
      $("#search_q").focus();
      return e.preventDefault();
    },
    scroller: function() {
      var scrollPos;
      scrollPos = window.scrollY;
      if (!this.community.animating && scrollPos > this.community.offsetTop) {
        this.community.animateSlideshow();
      }
    },
    albumFlipper: function() {
      var $albumFlipper, addCover, albumImgs, cur, getCovers, init;
      $albumFlipper = $(".gs_album_covers_bg");
      albumImgs = [];
      cur = 0;
      getCovers = function() {
        $.get(getUrl("/release/album_flipper_images?page=" + (cur + 1)), function(data) {
          var _ref1;
          if (data != null ? (_ref1 = data.images) != null ? _ref1.length : void 0 : void 0) {
            albumImgs = data.images;
            $albumFlipper.find("img:not(.loaded)").remove();
            addCover();
          }
          cur = __modulo(cur + 1, 10);
        });
      };
      addCover = function() {
        var $coverImg, $overlappedImg, coverImgLeft, coverImgTop, winW;
        winW = $(window).width();
        coverImgLeft = (Math.floor(Math.random() * ((winW / 75) / 2)) - Math.floor(Math.random() * ((winW / 75) / 2))) * 75;
        coverImgTop = Math.floor(Math.random() * (380 + 50) / 75) * 75 + 20;
        $coverImg = $("<img src=\"" + albumImgs[0] + "\" class=\"album_cover\" style=\"left: 50%; margin-left: " + coverImgLeft + "px; top: " + coverImgTop + "px;\" data-album-flipper-coords=\"" + coverImgLeft + "," + coverImgTop + "\" />");
        $overlappedImg = $albumFlipper.find("img[data-album-flipper-coords=\"" + coverImgLeft + "," + coverImgTop + "\"]");
        $albumFlipper.append($coverImg);
        $coverImg.on("load.albumFlipper", function() {
          if ($overlappedImg.length) {
            $overlappedImg.removeClass("loaded");
          }
          return $(this).addClass("loaded").off("load.albumFlipper");
        });
        albumImgs.splice(0, 1);
        if (albumImgs.length) {
          setTimeout(addCover, 1500);
        } else {
          getCovers();
        }
      };
      init = function() {
        if (!$albumFlipper.length || features.isMobile) {
          return;
        }
        getCovers();
      };
      return init();
    },
    videoShowcase: function() {
      var $videos, closeModal, init, resizeVideo, showModal, showVideos, ytPlayers;
      $videos = $("#gs_videos");
      ytPlayers = {};
      resizeVideo = function() {
        var $video;
        $video = $videos.find("iframe:visible");
        $video.css({
          height: $video.width() * ($video.attr("height") / $video.attr("width"))
        });
      };
      showModal = function(e) {
        var $whichModal, newYTPlayer, username, ytTracker;
        if (e) {
          e.preventDefault();
        }
        username = $(this).data("video-username");
        $whichModal = $("#gs_video_modal_" + username);
        if (!$whichModal.length) {
          return;
        }
        ytTracker = function(e) {
          var action;
          action = youtubeIframeAPI.getPlayerState(e);
          if (action === "playing" || action === "paused" || action === "ended") {
            util.track("get_started", "video_" + username + "_" + action);
          }
        };
        newYTPlayer = function() {
          ytPlayers[username] = new YT.Player("gs_video_" + username, {
            videoId: $whichModal.data("youtube-id"),
            playerVars: {
              controls: 2,
              modestbranding: 1,
              autoplay: 1
            },
            width: 560,
            height: 315,
            events: {
              onReady: resizeVideo,
              onStateChange: ytTracker
            }
          });
        };
        showVideos();
        $whichModal.show();
        youtubeIframeAPI.load().then(function() {
          if (!ytPlayers[username]) {
            newYTPlayer();
          }
        });
        $("html,body").animate({
          scrollTop: $whichModal.offset().top + (features.isMobile ? 0 : -20)
        }, 200);
        util.track("get_started", "modal_" + username + "_opened");
        setTimeout((function() {
          return $("body").on("click.gs_video_modal_close", function(e) {
            var $target;
            $target = $(e.target);
            if (!$target.closest(".gs_video_modal").length && !$target.closest(".ui-dialog").length) {
              return closeModal();
            }
          });
        }), 200);
      };
      showVideos = function(e) {
        $videos.find("#gs_videos_previews").addClass("open");
        events.trigger("parallax:calculate");
        if (e) {
          return e.preventDefault();
        }
      };
      closeModal = function(e) {
        var $video;
        if (e) {
          e.preventDefault();
        }
        $video = $videos.find(".gs_video_modal:visible");
        $video.hide();
        ytPlayers[$video.data("video-username")].pauseVideo();
        util.track("get_started", "modal_" + ($video.data("video-username")) + "_closed");
        $("body").off("click.gs_video_modal_close");
      };
      init = function() {
        if (!$videos.length) {
          return;
        }
        $videos.on("click", ".gs_videos_preview", showModal).on("click", "#gs_videos_previews_open", showVideos).on("click", ".gs_video_close", closeModal);
        $(window).on("resize", resizeVideo);
      };
      return init();
    },
    community: {
      instaSlideshow: function() {
        var imgTpl, instagramClientId;
        instagramClientId = "60efbf1fe64e42578e3f54b336b9e3d5";
        imgTpl = _.template($("#gs_community_img_tpl").html());
        $.ajax({
          url: "https://api.instagram.com/v1/tags/discogs/media/recent?client_id=" + instagramClientId,
          dataType: "jsonp",
          success: function(data) {
            var imgsHtml;
            imgsHtml = "";
            $.each(data.data, function(i, v) {
              return imgsHtml += imgTpl({
                link: v.link,
                imgSrc: v.images["low_resolution"].url,
                username: v.user.username
              });
            });
            pub.community.$slideshow.css("width", data.data.length * 254).append(imgsHtml);
            pub.community.amtToAnimate = pub.community.$slideshow.outerWidth(true) - $(window).width();
          }
        });
      },
      animateSlideshow: _.once(function(direction) {
        if (features.has3d()) {
          pub.community.$slideshow.addClass("animating");
        } else {
          pub.community.$slideshow.animate({
            left: (direction ? 0 : -pub.community.amtToAnimate)
          }, 60000, "linear", function() {
            pub.community.animateSlideshow(!direction);
          });
        }
      }),
      init: function() {
        this.$slideshow = $("#gs_community_slideshow");
        if (!this.$slideshow.length) {
          return;
        }
        this.offsetTop = $("#gs_community_wrapper").offset().top - $(window).height();
        this.instaSlideshow();
      }
    }
  };
  ready(function() {
    return pub.init();
  });
  return pub;
});
