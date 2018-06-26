(function ($) {
    var rotateLeft = function (lValue, iShiftBits) {
        return (lValue << iShiftBits) | (lValue >>> (32 - iShiftBits));
    }
    var addUnsigned = function (lX, lY) {
        var lX4, lY4, lX8, lY8, lResult;
        lX8 = (lX & 0x80000000);
        lY8 = (lY & 0x80000000);
        lX4 = (lX & 0x40000000);
        lY4 = (lY & 0x40000000);
        lResult = (lX & 0x3FFFFFFF) + (lY & 0x3FFFFFFF);
        if (lX4 & lY4) return (lResult ^ 0x80000000 ^ lX8 ^ lY8);
        if (lX4 | lY4) {
            if (lResult & 0x40000000) return (lResult ^ 0xC0000000 ^ lX8 ^ lY8);
            else return (lResult ^ 0x40000000 ^ lX8 ^ lY8);
        } else {
            return (lResult ^ lX8 ^ lY8);
        }
    }
    var F = function (x, y, z) {
        return (x & y) | ((~x) & z);
    }
    var G = function (x, y, z) {
        return (x & z) | (y & (~z));
    }
    var H = function (x, y, z) {
        return (x ^ y ^ z);
    }
    var I = function (x, y, z) {
        return (y ^ (x | (~z)));
    }
    var FF = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(F(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };
    var GG = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(G(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };
    var HH = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(H(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };
    var II = function (a, b, c, d, x, s, ac) {
        a = addUnsigned(a, addUnsigned(addUnsigned(I(b, c, d), x), ac));
        return addUnsigned(rotateLeft(a, s), b);
    };
    var convertToWordArray = function (string) {
        var lWordCount;
        var lMessageLength = string.length;
        var lNumberOfWordsTempOne = lMessageLength + 8;
        var lNumberOfWordsTempTwo = (lNumberOfWordsTempOne - (lNumberOfWordsTempOne % 64)) / 64;
        var lNumberOfWords = (lNumberOfWordsTempTwo + 1) * 16;
        var lWordArray = Array(lNumberOfWords - 1);
        var lBytePosition = 0;
        var lByteCount = 0;
        while (lByteCount < lMessageLength) {
            lWordCount = (lByteCount - (lByteCount % 4)) / 4;
            lBytePosition = (lByteCount % 4) * 8;
            lWordArray[lWordCount] = (lWordArray[lWordCount] | (string.charCodeAt(lByteCount) << lBytePosition));
            lByteCount++;
        }
        lWordCount = (lByteCount - (lByteCount % 4)) / 4;
        lBytePosition = (lByteCount % 4) * 8;
        lWordArray[lWordCount] = lWordArray[lWordCount] | (0x80 << lBytePosition);
        lWordArray[lNumberOfWords - 2] = lMessageLength << 3;
        lWordArray[lNumberOfWords - 1] = lMessageLength >>> 29;
        return lWordArray;
    };
    var wordToHex = function (lValue) {
        var WordToHexValue = "", WordToHexValueTemp = "", lByte, lCount;
        for (lCount = 0; lCount <= 3; lCount++) {
            lByte = (lValue >>> (lCount * 8)) & 255;
            WordToHexValueTemp = "0" + lByte.toString(16);
            WordToHexValue = WordToHexValue + WordToHexValueTemp.substr(WordToHexValueTemp.length - 2, 2);
        }
        return WordToHexValue;
    };
    var uTF8Encode = function (string) {
        string = string.replace(/\x0d\x0a/g, "\x0a");
        var output = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                output += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                output += String.fromCharCode((c >> 6) | 192);
                output += String.fromCharCode((c & 63) | 128);
            } else {
                output += String.fromCharCode((c >> 12) | 224);
                output += String.fromCharCode(((c >> 6) & 63) | 128);
                output += String.fromCharCode((c & 63) | 128);
            }
        }
        return output;
    };
    $.extend({
        md5: function (string) {
            var x = Array();
            var k, AA, BB, CC, DD, a, b, c, d;
            var S11 = 7, S12 = 12, S13 = 17, S14 = 22;
            var S21 = 5, S22 = 9, S23 = 14, S24 = 20;
            var S31 = 4, S32 = 11, S33 = 16, S34 = 23;
            var S41 = 6, S42 = 10, S43 = 15, S44 = 21;
            string = uTF8Encode(string);
            x = convertToWordArray(string);
            a = 0x67452301; b = 0xEFCDAB89; c = 0x98BADCFE; d = 0x10325476;
            for (k = 0; k < x.length; k += 16) {
                AA = a; BB = b; CC = c; DD = d;
                a = FF(a, b, c, d, x[k + 0], S11, 0xD76AA478);
                d = FF(d, a, b, c, x[k + 1], S12, 0xE8C7B756);
                c = FF(c, d, a, b, x[k + 2], S13, 0x242070DB);
                b = FF(b, c, d, a, x[k + 3], S14, 0xC1BDCEEE);
                a = FF(a, b, c, d, x[k + 4], S11, 0xF57C0FAF);
                d = FF(d, a, b, c, x[k + 5], S12, 0x4787C62A);
                c = FF(c, d, a, b, x[k + 6], S13, 0xA8304613);
                b = FF(b, c, d, a, x[k + 7], S14, 0xFD469501);
                a = FF(a, b, c, d, x[k + 8], S11, 0x698098D8);
                d = FF(d, a, b, c, x[k + 9], S12, 0x8B44F7AF);
                c = FF(c, d, a, b, x[k + 10], S13, 0xFFFF5BB1);
                b = FF(b, c, d, a, x[k + 11], S14, 0x895CD7BE);
                a = FF(a, b, c, d, x[k + 12], S11, 0x6B901122);
                d = FF(d, a, b, c, x[k + 13], S12, 0xFD987193);
                c = FF(c, d, a, b, x[k + 14], S13, 0xA679438E);
                b = FF(b, c, d, a, x[k + 15], S14, 0x49B40821);
                a = GG(a, b, c, d, x[k + 1], S21, 0xF61E2562);
                d = GG(d, a, b, c, x[k + 6], S22, 0xC040B340);
                c = GG(c, d, a, b, x[k + 11], S23, 0x265E5A51);
                b = GG(b, c, d, a, x[k + 0], S24, 0xE9B6C7AA);
                a = GG(a, b, c, d, x[k + 5], S21, 0xD62F105D);
                d = GG(d, a, b, c, x[k + 10], S22, 0x2441453);
                c = GG(c, d, a, b, x[k + 15], S23, 0xD8A1E681);
                b = GG(b, c, d, a, x[k + 4], S24, 0xE7D3FBC8);
                a = GG(a, b, c, d, x[k + 9], S21, 0x21E1CDE6);
                d = GG(d, a, b, c, x[k + 14], S22, 0xC33707D6);
                c = GG(c, d, a, b, x[k + 3], S23, 0xF4D50D87);
                b = GG(b, c, d, a, x[k + 8], S24, 0x455A14ED);
                a = GG(a, b, c, d, x[k + 13], S21, 0xA9E3E905);
                d = GG(d, a, b, c, x[k + 2], S22, 0xFCEFA3F8);
                c = GG(c, d, a, b, x[k + 7], S23, 0x676F02D9);
                b = GG(b, c, d, a, x[k + 12], S24, 0x8D2A4C8A);
                a = HH(a, b, c, d, x[k + 5], S31, 0xFFFA3942);
                d = HH(d, a, b, c, x[k + 8], S32, 0x8771F681);
                c = HH(c, d, a, b, x[k + 11], S33, 0x6D9D6122);
                b = HH(b, c, d, a, x[k + 14], S34, 0xFDE5380C);
                a = HH(a, b, c, d, x[k + 1], S31, 0xA4BEEA44);
                d = HH(d, a, b, c, x[k + 4], S32, 0x4BDECFA9);
                c = HH(c, d, a, b, x[k + 7], S33, 0xF6BB4B60);
                b = HH(b, c, d, a, x[k + 10], S34, 0xBEBFBC70);
                a = HH(a, b, c, d, x[k + 13], S31, 0x289B7EC6);
                d = HH(d, a, b, c, x[k + 0], S32, 0xEAA127FA);
                c = HH(c, d, a, b, x[k + 3], S33, 0xD4EF3085);
                b = HH(b, c, d, a, x[k + 6], S34, 0x4881D05);
                a = HH(a, b, c, d, x[k + 9], S31, 0xD9D4D039);
                d = HH(d, a, b, c, x[k + 12], S32, 0xE6DB99E5);
                c = HH(c, d, a, b, x[k + 15], S33, 0x1FA27CF8);
                b = HH(b, c, d, a, x[k + 2], S34, 0xC4AC5665);
                a = II(a, b, c, d, x[k + 0], S41, 0xF4292244);
                d = II(d, a, b, c, x[k + 7], S42, 0x432AFF97);
                c = II(c, d, a, b, x[k + 14], S43, 0xAB9423A7);
                b = II(b, c, d, a, x[k + 5], S44, 0xFC93A039);
                a = II(a, b, c, d, x[k + 12], S41, 0x655B59C3);
                d = II(d, a, b, c, x[k + 3], S42, 0x8F0CCC92);
                c = II(c, d, a, b, x[k + 10], S43, 0xFFEFF47D);
                b = II(b, c, d, a, x[k + 1], S44, 0x85845DD1);
                a = II(a, b, c, d, x[k + 8], S41, 0x6FA87E4F);
                d = II(d, a, b, c, x[k + 15], S42, 0xFE2CE6E0);
                c = II(c, d, a, b, x[k + 6], S43, 0xA3014314);
                b = II(b, c, d, a, x[k + 13], S44, 0x4E0811A1);
                a = II(a, b, c, d, x[k + 4], S41, 0xF7537E82);
                d = II(d, a, b, c, x[k + 11], S42, 0xBD3AF235);
                c = II(c, d, a, b, x[k + 2], S43, 0x2AD7D2BB);
                b = II(b, c, d, a, x[k + 9], S44, 0xEB86D391);
                a = addUnsigned(a, AA);
                b = addUnsigned(b, BB);
                c = addUnsigned(c, CC);
                d = addUnsigned(d, DD);
            }
            var tempValue = wordToHex(a) + wordToHex(b) + wordToHex(c) + wordToHex(d);
            return tempValue.toLowerCase();
        }
    });
})(jQuery);

jQuery.cookie = function (name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        // CAUTION: Needed to parenthesize options.path and options.domain
        // in the following expressions, otherwise they evaluate to undefined
        // in the packed version for some reason...
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

//a标签的重定向页面载入效果
(function (a) {
    a.flexslider = function (c, b) {
        var d = a(c);
        a.data(c, "flexslider", d);
        d.init = function () {
            d.vars = a.extend({}, a.flexslider.defaults, b);
            a.data(c, "flexsliderInit", true);
            d.container = a(".slides", d).first();
            d.slides = a(".slides:first > li", d);
            d.count = d.slides.length;
            d.animating = false;
            d.currentSlide = d.vars.slideToStart;
            d.animatingTo = d.currentSlide;
            d.atEnd = d.currentSlide == 0 ? true : false;
            d.eventType = "ontouchstart" in document.documentElement ? "touchstart" : "click";
            d.cloneCount = 0;
            d.cloneOffset = 0;
            d.manualPause = false;
            d.vertical = d.vars.slideDirection == "vertical";
            d.prop = d.vertical ? "top" : "marginLeft";
            d.args = {};
            d.transitions = "webkitTransition" in document.body.style;
            if (d.transitions) {
                d.prop = "-webkit-transform";
            }
            if (d.vars.controlsContainer != "") {
                d.controlsContainer = a(d.vars.controlsContainer).eq(a(".slides").index(d.container));
                d.containerExists = d.controlsContainer.length > 0;
            }
            if (d.vars.manualControls != "") {
                d.manualControls = a(d.vars.manualControls, d.containerExists ? d.controlsContainer : d);
                d.manualExists = d.manualControls.length > 0;
            }
            if (d.vars.randomize) {
                d.slides.sort(function () {
                    return Math.round(Math.random()) - .5;
                });
                d.container.empty().append(d.slides);
            }
            if (d.vars.animation.toLowerCase() == "slide") {
                if (d.transitions) {
                    d.setTransition(0);
                }
                d.css({
                    overflow: "hidden"
                });
                if (d.vars.animationLoop) {
                    d.cloneCount = 2;
                    d.cloneOffset = 1;
                    d.container.append(d.slides.filter(":first").clone().addClass("clone")).prepend(d.slides.filter(":last").clone().addClass("clone"));
                }
                d.newSlides = a(".slides:first > li", d);
                var m = -1 * (d.currentSlide + d.cloneOffset);
                if (d.vertical) {
                    d.newSlides.css({
                        display: "block",
                        width: "100%",
                        "float": "left"
                    });
                    d.container.height((d.count + d.cloneCount) * 200 + "%").css("position", "absolute").width("100%");
                    setTimeout(function () {
                        d.css({
                            position: "relative"
                        }).height(d.slides.filter(":first").height());
                        d.args[d.prop] = d.transitions ? "translate3d(0," + m * d.height() + "px,0)" : m * d.height() + "px";
                        d.container.css(d.args);
                    }, 100);
                } else {
                    d.args[d.prop] = d.transitions ? "translate3d(" + m * d.width() + "px,0,0)" : m * d.width() + "px";
                    d.container.width((d.count + d.cloneCount) * 200 + "%").css(d.args);
                    setTimeout(function () {
                        d.newSlides.width(d.width()).css({
                            "float": "left",
                            display: "block"
                        });
                    }, 100);
                }
            } else {
                d.transitions = false;
                d.slides.css({
                    width: "100%",
                    "float": "left",
                    marginRight: "-100%"
                }).eq(d.currentSlide).fadeIn(d.vars.animationDuration);
            }
            if (d.vars.controlNav) {
                if (d.manualExists) {
                    d.controlNav = d.manualControls;
                } else {
                    var e = a('<ol class="flex-control-nav"></ol>');
                    var s = 1;
                    for (var t = 0; t < d.count; t++) {
                        e.append("<li><a>" + s + "</a></li>");
                        s++;
                    }
                    if (d.containerExists) {
                        a(d.controlsContainer).append(e);
                        d.controlNav = a(".flex-control-nav li a", d.controlsContainer);
                    } else {
                        d.append(e);
                        d.controlNav = a(".flex-control-nav li a", d);
                    }
                }
                d.controlNav.eq(d.currentSlide).addClass("active");
                d.controlNav.bind(d.eventType, function (i) {
                    i.preventDefault();
                    if (!a(this).hasClass("active")) {
                        d.controlNav.index(a(this)) > d.currentSlide ? d.direction = "next" : d.direction = "prev";
                        d.flexAnimate(d.controlNav.index(a(this)), d.vars.pauseOnAction);
                    }
                });
            }
            if (d.vars.directionNav) {
                var v = a('<ul class="flex-direction-nav"><li><a class="prev" href="#">' + d.vars.prevText + '</a></li><li><a class="next" href="#">' + d.vars.nextText + "</a></li></ul>");
                if (d.containerExists) {
                    a(d.controlsContainer).append(v);
                    d.directionNav = a(".flex-direction-nav li a", d.controlsContainer);
                } else {
                    d.append(v);
                    d.directionNav = a(".flex-direction-nav li a", d);
                }
                if (!d.vars.animationLoop) {
                    if (d.currentSlide == 0) {
                        d.directionNav.filter(".prev").addClass("disabled");
                    } else {
                        if (d.currentSlide == d.count - 1) {
                            d.directionNav.filter(".next").addClass("disabled");
                        }
                    }
                }
                d.directionNav.bind(d.eventType, function (i) {
                    i.preventDefault();
                    var j = a(this).hasClass("next") ? d.getTarget("next") : d.getTarget("prev");
                    if (d.canAdvance(j)) {
                        d.flexAnimate(j, d.vars.pauseOnAction);
                    }
                });
            }
            if (d.vars.keyboardNav && a("ul.slides").length == 1) {
                function h(i) {
                    if (d.animating) {
                        return;
                    } else {
                        if (i.keyCode != 39 && i.keyCode != 37) {
                            return;
                        } else {
                            if (i.keyCode == 39) {
                                var j = d.getTarget("next");
                            } else {
                                if (i.keyCode == 37) {
                                    var j = d.getTarget("prev");
                                }
                            }
                            if (d.canAdvance(j)) {
                                d.flexAnimate(j, d.vars.pauseOnAction);
                            }
                        }
                    }
                }
                a(document).bind("keyup", h);
            }
            if (d.vars.mousewheel) {
                d.mousewheelEvent = /Firefox/i.test(navigator.userAgent) ? "DOMMouseScroll" : "mousewheel";
                d.bind(d.mousewheelEvent, function (y) {
                    y.preventDefault();
                    y = y ? y : window.event;
                    var i = y.detail ? y.detail * -1 : y.originalEvent.wheelDelta / 40, j = i < 0 ? d.getTarget("next") : d.getTarget("prev");
                    if (d.canAdvance(j)) {
                        d.flexAnimate(j, d.vars.pauseOnAction);
                    }
                });
            }
            if (d.vars.slideshow) {
                if (d.vars.pauseOnHover && d.vars.slideshow) {
                    d.hover(function () {
                        d.pause();
                    }, function () {
                        if (!d.manualPause) {
                            d.resume();
                        }
                    });
                }
                d.animatedSlides = setInterval(d.animateSlides, d.vars.slideshowSpeed);
            }
            if (d.vars.pausePlay) {
                var q = a('<div class="flex-pauseplay"><span></span></div>');
                if (d.containerExists) {
                    d.controlsContainer.append(q);
                    d.pausePlay = a(".flex-pauseplay span", d.controlsContainer);
                } else {
                    d.append(q);
                    d.pausePlay = a(".flex-pauseplay span", d);
                }
                var n = d.vars.slideshow ? "pause" : "play";
                d.pausePlay.addClass(n).text(n == "pause" ? d.vars.pauseText : d.vars.playText);
                d.pausePlay.bind(d.eventType, function (i) {
                    i.preventDefault();
                    if (a(this).hasClass("pause")) {
                        d.pause();
                        d.manualPause = true;
                    } else {
                        d.resume();
                        d.manualPause = false;
                    }
                });
            }
            if ("ontouchstart" in document.documentElement) {
                var w, u, l, r, o, x, p = false;
                d.each(function () {
                    if ("ontouchstart" in document.documentElement) {
                        this.addEventListener("touchstart", g, false);
                    }
                });
                function g(i) {
                    if (d.animating) {
                        i.preventDefault();
                    } else {
                        if (i.touches.length == 1) {
                            d.pause();
                            r = d.vertical ? d.height() : d.width();
                            x = Number(new Date());
                            l = d.vertical ? (d.currentSlide + d.cloneOffset) * d.height() : (d.currentSlide + d.cloneOffset) * d.width();
                            w = d.vertical ? i.touches[0].pageY : i.touches[0].pageX;
                            u = d.vertical ? i.touches[0].pageX : i.touches[0].pageY;
                            d.setTransition(0);
                            this.addEventListener("touchmove", k, false);
                            this.addEventListener("touchend", f, false);
                        }
                    }
                }
                function k(i) {
                    o = d.vertical ? w - i.touches[0].pageY : w - i.touches[0].pageX;
                    p = d.vertical ? Math.abs(o) < Math.abs(i.touches[0].pageX - u) : Math.abs(o) < Math.abs(i.touches[0].pageY - u);
                    if (!p) {
                        i.preventDefault();
                        if (d.vars.animation == "slide" && d.transitions) {
                            if (!d.vars.animationLoop) {
                                o = o / (d.currentSlide == 0 && o < 0 || d.currentSlide == d.count - 1 && o > 0 ? Math.abs(o) / r + 2 : 1);
                            }
                            d.args[d.prop] = d.vertical ? "translate3d(0," + (-l - o) + "px,0)" : "translate3d(" + (-l - o) + "px,0,0)";
                            d.container.css(d.args);
                        }
                    }
                }
                function f(j) {
                    d.animating = false;
                    if (d.animatingTo == d.currentSlide && !p && !(o == null)) {
                        var i = o > 0 ? d.getTarget("next") : d.getTarget("prev");
                        if (d.canAdvance(i) && Number(new Date()) - x < 550 && Math.abs(o) > 20 || Math.abs(o) > r / 2) {
                            d.flexAnimate(i, d.vars.pauseOnAction);
                        } else {
                            d.flexAnimate(d.currentSlide, d.vars.pauseOnAction);
                        }
                    }
                    this.removeEventListener("touchmove", k, false);
                    this.removeEventListener("touchend", f, false);
                    w = null;
                    u = null;
                    o = null;
                    l = null;
                }
            }
            if (d.vars.animation.toLowerCase() == "slide") {
                a(window).resize(function () {
                    if (!d.animating && d.is(":visible")) {
                        if (d.vertical) {
                            d.height(d.slides.filter(":first").height());
                            d.args[d.prop] = -1 * (d.currentSlide + d.cloneOffset) * d.slides.filter(":first").height() + "px";
                            if (d.transitions) {
                                d.setTransition(0);
                                d.args[d.prop] = d.vertical ? "translate3d(0," + d.args[d.prop] + ",0)" : "translate3d(" + d.args[d.prop] + ",0,0)";
                            }
                            d.container.css(d.args);
                        } else {
                            d.newSlides.width(d.width());
                            d.args[d.prop] = -1 * (d.currentSlide + d.cloneOffset) * d.width() + "px";
                            if (d.transitions) {
                                d.setTransition(0);
                                d.args[d.prop] = d.vertical ? "translate3d(0," + d.args[d.prop] + ",0)" : "translate3d(" + d.args[d.prop] + ",0,0)";
                            }
                            d.container.css(d.args);
                        }
                    }
                });
            }

            var back_color = d.slides.eq(d.currentSlide).attr("back");
            $("#sec_famous").css("background-color", back_color);

            d.vars.start(d);
        };
        d.flexAnimate = function (g, f) {
            if (!d.animating && d.is(":visible")) {
                d.animating = true;
                d.animatingTo = g;
                d.vars.before(d);
                if (f) {
                    d.pause();
                }
                if (d.vars.controlNav) {
                    d.controlNav.removeClass("active").eq(g).addClass("active");
                }
                d.atEnd = g == 0 || g == d.count - 1 ? true : false;

                if (!d.vars.animationLoop && d.vars.directionNav) {
                    if (g == 0) {
                        d.directionNav.removeClass("disabled").filter(".prev").addClass("disabled");
                    } else {
                        if (g == d.count - 1) {
                            d.directionNav.removeClass("disabled").filter(".next").addClass("disabled");
                        } else {
                            d.directionNav.removeClass("disabled");
                        }
                    }
                }
                if (!d.vars.animationLoop && g == d.count - 1) {
                    d.pause();
                    d.vars.end(d);
                }

                if (d.vars.animation.toLowerCase() == "slide") {
                    var e = d.vertical ? d.slides.filter(":first").height() : d.slides.filter(":first").width();
                    if (d.currentSlide == 0 && g == d.count - 1 && d.vars.animationLoop && d.direction != "next") {
                        d.slideString = "0px";
                    } else {
                        if (d.currentSlide == d.count - 1 && g == 0 && d.vars.animationLoop && d.direction != "prev") {
                            d.slideString = -1 * (d.count + 1) * e + "px";
                        } else {
                            d.slideString = -1 * (g + d.cloneOffset) * e + "px";
                        }
                    }
                    d.args[d.prop] = d.slideString;
                    if (d.transitions) {
                        d.setTransition(d.vars.animationDuration);
                        d.args[d.prop] = d.vertical ? "translate3d(0," + d.slideString + ",0)" : "translate3d(" + d.slideString + ",0,0)";
                        d.container.css(d.args).one("webkitTransitionEnd transitionend", function () {
                            d.wrapup(e);
                        });
                    } else {
                        d.container.animate(d.args, d.vars.animationDuration, function () {
                            d.wrapup(e);
                        });
                    }
                } else {
                    d.slides.eq(d.currentSlide).fadeOut(d.vars.animationDuration);
                    d.slides.eq(g).fadeIn(d.vars.animationDuration, function () {
                        d.wrapup();
                    });
                }
            }

        };
        d.wrapup = function (e) {
            if (d.vars.animation == "slide") {
                if (d.currentSlide == 0 && d.animatingTo == d.count - 1 && d.vars.animationLoop) {
                    d.args[d.prop] = -1 * d.count * e + "px";
                    if (d.transitions) {
                        d.setTransition(0);
                        d.args[d.prop] = d.vertical ? "translate3d(0," + d.args[d.prop] + ",0)" : "translate3d(" + d.args[d.prop] + ",0,0)";
                    }
                    d.container.css(d.args);
                } else {
                    if (d.currentSlide == d.count - 1 && d.animatingTo == 0 && d.vars.animationLoop) {
                        d.args[d.prop] = -1 * e + "px";
                        if (d.transitions) {
                            d.setTransition(0);
                            d.args[d.prop] = d.vertical ? "translate3d(0," + d.args[d.prop] + ",0)" : "translate3d(" + d.args[d.prop] + ",0,0)";
                        }
                        d.container.css(d.args);
                    }
                }
            }
            d.animating = false;
            d.currentSlide = d.animatingTo;
            d.vars.after(d);

            var back_color = d.slides.eq(d.currentSlide).attr("back");
            $("#sec_famous").css("background-color", back_color);
        };
        d.animateSlides = function () {
            if (!d.animating) {
                d.flexAnimate(d.getTarget("next"));
            }
        };
        d.pause = function () {
            clearInterval(d.animatedSlides);
            if (d.vars.pausePlay) {
                d.pausePlay.removeClass("pause").addClass("play").text(d.vars.playText);
            }
        };
        d.resume = function () {
            d.animatedSlides = setInterval(d.animateSlides, d.vars.slideshowSpeed);
            if (d.vars.pausePlay) {
                d.pausePlay.removeClass("play").addClass("pause").text(d.vars.pauseText);
            }
        };
        d.canAdvance = function (e) {
            if (!d.vars.animationLoop && d.atEnd) {
                if (d.currentSlide == 0 && e == d.count - 1 && d.direction != "next") {
                    return false;
                } else {
                    if (d.currentSlide == d.count - 1 && e == 0 && d.direction == "next") {
                        return false;
                    } else {
                        return true;
                    }
                }
            } else {
                return true;
            }
        };
        d.getTarget = function (e) {
            d.direction = e;
            if (e == "next") {
                return d.currentSlide == d.count - 1 ? 0 : d.currentSlide + 1;
            } else {
                return d.currentSlide == 0 ? d.count - 1 : d.currentSlide - 1;
            }
        };
        d.setTransition = function (e) {
            d.container.css({
                "-webkit-transition-duration": e / 1e3 + "s"
            });
        };
        d.init();
    };
    a.flexslider.defaults = {
        animation: "fade",
        slideDirection: "horizontal",
        slideshow: true,
        slideshowSpeed: 7e3,
        animationDuration: 600,
        directionNav: true,
        controlNav: true,
        keyboardNav: true,
        mousewheel: false,
        prevText: "Previous",
        nextText: "Next",
        pausePlay: false,
        pauseText: "Pause",
        playText: "Play",
        randomize: false,
        slideToStart: 0,
        animationLoop: true,
        pauseOnAction: true,
        pauseOnHover: false,
        controlsContainer: "",
        manualControls: "",
        start: function () { },
        before: function () { },
        after: function () { },
        end: function () { }
    };
    a.fn.flexslider = function (b) {
        return this.each(function () {
            if (a(this).find(".slides li").length == 1) {
                a(this).find(".slides li").fadeIn(400);
            } else {
                if (a(this).data("flexsliderInit") != true) {
                    new a.flexslider(this, b);
                }
            }
        });
    };
})(jQuery);

//购物车数量+1的动画效果
(function ($) {
    $.extend({
        tipsBox: function (options) {
            options = $.extend({
                obj: null,  //jq对象，要在那个html标签上显示
                str: "+1",  //字符串，要显示的内容;也可以传一段html，如: "<b style='font-family:Microsoft YaHei;'>+1</b>"
                startSize: "12px",  //动画开始的文字大小
                endSize: "30px",    //动画结束的文字大小
                interval: 600,  //动画时间间隔
                color: "red",    //文字颜色
                callback: function () { }    //回调函数
            }, options);
            $("body").append("<span class='num'>" + options.str + "</span>");
            var box = $(".num");
            var left = options.obj.offset().left + options.obj.width() / 2;
            var top = options.obj.offset().top - options.obj.height();
            box.css({
                "position": "absolute",
                "left": left + "px",
                "top": top + "px",
                "z-index": 9999,
                "font-size": options.startSize,
                "line-height": options.endSize,
                "color": options.color
            });
            box.animate({
                "font-size": options.endSize,
                "opacity": "0",
                "top": top - parseInt(options.endSize) + "px"
            }, options.interval, function () {
                box.remove();
                options.callback();
            });
        }
    });
})(jQuery);

$.fn.countTo = function (options) {
    options = options || {};
    return $(this).each(function () {
        // set options for current element
        var settings = $.extend({}, $.fn.countTo.defaults, {
            from: $(this).data('from'),
            to: $(this).data('to'),
            speed: $(this).data('speed'),
            refreshInterval: $(this).data('refresh-interval'),
            decimals: $(this).data('decimals')
        }, options);
        // how many times to update the value, and how much to increment the value on each update
        var loops = Math.ceil(settings.speed / settings.refreshInterval),
            increment = (settings.to - settings.from) / loops;
        // references & variables that will change with each update
        var self = this,
            $self = $(this),
            loopCount = 0,
            value = settings.from,
            data = $self.data('countTo') || {};
        $self.data('countTo', data);
        // if an existing interval can be found, clear it first
        if (data.interval) {
            clearInterval(data.interval);
        }
        data.interval = setInterval(updateTimer, settings.refreshInterval);
        // initialize the element with the starting value
        render(value);
        function updateTimer() {
            value += increment;
            loopCount++;
            render(value);
            if (typeof (settings.onUpdate) == 'function') {
                settings.onUpdate.call(self, value);
            }
            if (loopCount >= loops) {
                // remove the interval
                $self.removeData('countTo');
                clearInterval(data.interval);
                value = settings.to;
                if (typeof (settings.onComplete) == 'function') {
                    settings.onComplete.call(self, value);
                }
            }
        }
        function render(value) {
            var formattedValue = settings.formatter.call(self, value, settings);
            $self.html(formattedValue);
        }
    });
};
$.fn.countTo.defaults = {
    from: 0,               // the number the element should start at
    to: 0,                 // the number the element should end at
    speed: 1000,           // how long it should take to count between the target numbers
    refreshInterval: 100,  // how often the element should be updated
    decimals: 0,           // the number of decimal places to show
    formatter: formatter,  // handler for formatting the value before rendering
    onUpdate: null,        // callback method for every time the element is updated
    onComplete: null       // callback method for when the element finishes updating
};
function formatter(value, settings) {
    return value.toFixed(settings.decimals);
}
// custom formatting example
$('[name=myNumCount]').data('countToOptions', {
    formatter: function (value, options) {
        return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
    }
});
function count(options) {
    var $this = $(this);
    options = $.extend({}, options || {}, $this.data('countToOptions') || {});
    $this.countTo(options);
}

//封装的获取URL地址中某个参数的方法
function GetUrlParam(myUrl, myName) {
    var reg = new RegExp("(^|&)" + myName + "=([^&]*)(&|$)");
    var r = myUrl.substr(1).match(reg);
    if (r != null) return decodeURIComponent(r[2]); return null;
}
//加载时候的提示框
function ShowLoader(myText) {
    var myVisible = true;
    if (myText.length <= 0) {
        myVisible = false;
    }
    $.mobile.loading("show", {
        text: myText,
        textVisible: myVisible,
        theme: "b",
        textonly: false,
        html: ""
    });
}
//隐藏加载的提示框
function HideLoader() {
    $.mobile.loading("hide");
}
function ShowErrLoader() {
    //隐藏加载器  
    $.mobile.loading('hide');
    $.mobile.loading("show", {
        text: '加载失败', //加载器中显示的文字  
        textVisible: true, //是否显示文字  
        theme: 'b',        //加载器主题样式a-e  
        textonly: false,   //是否只显示文字  
        html: ""           //要显示的html内容，如图片等 
    });
    setTimeout(function () {
        $.mobile.loading('hide');
    }, 1000);
} 
function ShowNoteLoader(textVal) {
    //隐藏加载器  
    $.mobile.loading('hide');
    $.mobile.loading("show", {
        text: textVal, //加载器中显示的文字  
        textVisible: true, //是否显示文字  
        theme: 'b',        //加载器主题样式a-e  
        textonly: false,   //是否只显示文字  
        html: ""           //要显示的html内容，如图片等 
    });
    setTimeout(function () {
        $.mobile.loading('hide');
    }, 1000);
}//可以用来做消息提示
function ShowSuccessLoader(textVal) {
    //隐藏加载器  
    $.mobile.loading('hide');
    $.mobile.loading("show", {
        text: textVal, //加载器中显示的文字  
        textVisible: true, //是否显示文字  
        theme: 'b',        //加载器主题样式a-e  
        textonly: true,   //是否只显示文字  
        html: ""           //要显示的html内容，如图片等 
    });
    setTimeout(function () {
        $.mobile.loading('hide');
    }, 1000);
}//可以用来做消息提示

//显示错误提示框信息
function ShowPromptBar(currentPage, promptText) {
    currentPage.find("#divPromptBar").animate({ top: -currentPage.find("#divPromptBar").outerHeight() }, 0);
    currentPage.find("#pPromptBar").text($.trim(promptText));
    currentPage.find("#divPromptBar").show();
    currentPage.find("#divPromptBar").animate({ top: "0" }, 400);
    setTimeout(function () {
        $("[id=divPromptBar]").each(function () {
            $(this).animate({ top: -$(this).outerHeight() }, 200);
        });
    }, 1800);
}
//隐藏错误提示框信息
function HidePromptBar() {
    $("[id=divPromptBar]").each(function () {
        $(this).animate({ top: -$(this).outerHeight() }, 200);
    });
}
//显示选择按钮提示框
function ShowShowbox(currentPage, showboxType, showboxText, showboxColor, showboxLeft, showboxRight, showboxHref, showboxTransition) {
    if ($.trim(showboxType) == "success") {
        currentPage.find("#imgShowbox").attr("alt", "success");
        currentPage.find("#imgShowbox").attr("src", "/images/web/icon_success.png");
    }
    else {
        currentPage.find("#imgShowbox").attr("alt", "failure");
        currentPage.find("#imgShowbox").attr("src", "/images/web/icon_failure.png");
    }
    currentPage.find("#pShowbox").text($.trim(showboxText));
    currentPage.find("#btnShowboxLeft").css("background-color", $.trim(showboxColor));
    currentPage.find("#btnShowboxLeft").text($.trim(showboxLeft));
    currentPage.find("#btnShowboxRight").text($.trim(showboxRight));
    currentPage.find("#btnShowboxLeft").attr("href", $.trim(showboxHref));
    currentPage.find("#btnShowboxLeft").attr("data-transition", $.trim(showboxTransition));
    currentPage.find("#divShowboxBack").width($(document).width());
    currentPage.find("#divShowboxBack").height($(document).height());
    currentPage.find("#divShowboxBack").addClass("is-visible");
}
//隐藏选择按钮提示框
function divShowboxHide_Click() {
    $("[id=divShowboxBack]").each(function () {
        $(this).removeClass("is-visible");
    });
}
//提示框显示
function ShowNoRecord(currentPage, recordText) {
    currentPage.find("#pNoRecord").text($.trim(recordText));
    currentPage.find("#divNoRecord").show();
}
//提示框隐藏
function HideNoRecord(currentPage) {
    currentPage.find("#divNoRecord").hide();
    currentPage.find("#pNoRecord").text("");
}
//给页脚添加被选外观
function ActiveFooter(currentPage, footerId) {
    currentPage.find("[id^=btnFooter]").removeClass("ui-btn-active");
    if (footerId == 1) {
        currentPage.find("#btnFooterOne").eq(0).off("onclick").on("click", function () {
            return false;
        });
        currentPage.find("#btnFooterOne").eq(0).find("img").attr("src", "../images/web/hpage/index2.png");
    } else if (footerId == 2) {
        currentPage.find("#btnFooterTwo").eq(0).find("img").attr("src", "../images/web/hpage/category_2.png");
    } else if(footerId==3){
        currentPage.find("#btnFooterThree").eq(0).find("img").attr("src", "../images/web/hpage/cart_2.png");
    }else if(footerId==4){
        currentPage.find("#btnFooterFour").eq(0).find("img").attr("src", "../images/web/hpage/myhome2.png");
    } else if (footerId == 5) {
        currentPage.find("#btnFooterFive").eq(0).find("img").attr("src", "../images/web/hpage/custservice2.png");
    }
}
function btnDeleteCart_Click(cartID) {
    var currentPage = $("[id=divCartList]:last");
    currentPage.find("#btnShowboxLeft").attr("onclick", "btnDeleteCartOk_Click('" + cartID + "')");
    ShowShowbox(currentPage, "failure", "确定要删除吗？", "#6699cc", "是", "否", "#", "slidefade");
    return false;
}
//确认删除购物车该条记录
function btnDeleteCartOk_Click(cartID) {
    var currentPage = $("[id=divCartList]:last");
        var url = "/web/ajax.aspx";
        var args = { "jsonType": "deleteUserCart", "id":cartID, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data == "UNLOGIN") {
                $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
            }
            else {
                if (data == "SUCCESS") {
                    $("[id^=liCartGoodsList_" + cartID + "]").remove();
                    if ($("[id^=liCartGoodsList_]").length <= 0) {
                        currentPage.find("#ulUserCart").hide();
                        ShowNoRecord(currentPage, "您的购物车为空");
                    }
                    var cartTotalPrice = 0;
                    currentPage.find("[id^=bSubtotalPrice_]").each(function () {
                        cartTotalPrice += Number($.trim($(this).text()));
                    });
                    currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
                    divShowboxHide_Click();
                } else {
                    ShowPromptBar(currentPage, "删除失败，请重试");
                }
            }
        });
    return false;
}
//购物车跳转产品信息
function btnToShowPage_Click(cart_ID,goodsID) {
    if ($.trim($("[id^=btnToShowPage_" + cart_ID + "]").data("change")) == $.trim($("[id^=tbNumber_" + cart_ID + "]").data("change"))) {//防止点击+—号时进行跳转
        $.mobile.changePage("/web/goods_show.aspx?goodsid=" + goodsID, { transition: "slidefade" });
    }
    else {
        $("[id^=btnToShowPage_" + cart_ID + "]").data("change", $.trim($("[id^=tbNumber_" + cart_ID + "]").data("change")));
    }
    return false;
}
//购物车商品数量更改
function btnChangeCartNumber_Click(goodsType, cart_ID, changeType) {
    if (changeType == "reduce" && Number($.trim($("[id^=tbNumber_" + cart_ID + "]").val())) > 1) {
        $.tipsBox({
            obj: $("[id^=btnReduce_" + cart_ID + "]"),
            str: "-1",
            callback: function () {
            }
        });
    }
    else if (changeType == "add" && Number($.trim($("[id^=tbNumber_" + cart_ID + "]").val())) < 99) {
        $.tipsBox({
            obj: $("[id^=btnAdd_" + cart_ID + "]"),
            str: "+1",
            callback: function () {
            }
        });
    }
    else if (changeType == "reduce" && Number($.trim($("[id^=tbNumber_" + cart_ID + "]").val())) == 1) {
        btnDeleteCart_Click(cart_ID);//删除购物车该件商品
    }
    var currentPage = $("[id=divCartList]:last");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "changeCartNumber", "id": cart_ID, "type": changeType, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data == "UNLOGIN") {
            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
        }
        else {
            if (data.indexOf("ERROR") >= 0) {
                ShowNoteLoader(data);
            } else {
                $("[id^=tbNumber_" + cart_ID + "]").val($.trim(data));
                $("[id^=bSubtotalPrice_" + cart_ID + "]").text((Number($.trim($("[id^=emGoodsPrice_" + cart_ID + "]").text())) * Number($.trim(data))).toFixed(2));
                var cartTotalPrice = 0;
                currentPage.find("[id^=bSubtotalPrice_]").each(function () {
                    if ($(this).parents("li").find("[name=layout]").eq(0).is(':checked')) {
                        cartTotalPrice += Number($.trim($(this).text()));
                    }
                });
                currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
            }
        }
    });
    return false;
}
//购物车商品数量更改input标签
function ChangeCartNumber(obj) {
    var currentPage = $("[id=divCartList]:last");
    var number =$.trim( $(obj).val());
    var cart_ID = $.trim($(obj).attr("mydata"));
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "changeCartNumbers", "id": cart_ID, "number": number, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data == "UNLOGIN") {
            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
        }
        else {
            if (data.indexOf("ERROR") >= 0) {
                ShowPromptBar(currentPage,data);
            } else {
                $("[id^=tbNumber_" + cart_ID + "]").val($.trim(data));
                $("[id^=bSubtotalPrice_" + cart_ID + "]").text((Number($.trim($("[id^=emGoodsPrice_" + cart_ID + "]").text())) * Number($.trim(data))).toFixed(2));
                var cartTotalPrice = 0;
                currentPage.find("[id^=bSubtotalPrice_]").each(function () {
                    if ($(this).parents("li").find("[name=layout]").eq(0).is(':checked')) {
                        cartTotalPrice += Number($.trim($(this).text()));
                    }
                });
                currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
            }
        }
    });
    return false;
}

//确认删除地址
function btnDeleteAddressOk_Click(addressId) {
    var currentPage = $("[id=divPageUserAddress]:last");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "deleteUserAddress", "id": addressId, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            $.mobile.changePage("/web/user_address.aspx", { transition: "slidefade" });
        }
        else {
            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
        }
        divShowboxHide_Click();
    });
    return false;
}
//首页产品显示
function GetTopGoodsList(gridCount, goodsId, goodsPhoto, goodsName, goodsPrice, goodsMarketPrice,goodsNumber) {
    var listLi = "<div class=\"ui-block-" + gridCount + "\"><div class=\"index-top\"><a href=\"/web/goods_show.aspx?goodsId=" + goodsId + "\" class=\"top-border\" data-transition=\"slidefade\">";
    if (goodsNumber > 0) {
        listLi += "<div class=\"top-photo\"><img alt=\"pic\" src=\"" + goodsPhoto + "\" onerror=\"nofind()\"/></div>";
    } else {
        listLi += "<div class=\"top-photo\"><img alt=\"pic\" src=\"" + goodsPhoto + "\" onerror=\"nofind()\"/><span class=\"top_soldout_css\"><img src=\"/images/web/hpage/soldoutRed.png\"/></span></div>";
    }
    //listLi += "<div class=\"top-photo\"><img alt=\"pic\" src=\"" + goodsPhoto + "\" onerror=\"this.src='nofind()\"/></div>";
    listLi += "<div class=\"top-name\" style=\"white-space: nowrap; overflow: hidden;text-overflow: ellipsis;\" >" + goodsName + "</div>";
    listLi += "<div class=\"top-price\">&yen;" + goodsPrice + "<p><strong>&yen;" + goodsMarketPrice + "</strong></p></div></a></div></div>";
    return listLi;
}
//img标签的onerror事件-加载不了图片使用默认的替换图片进行显示
function nofind() {
    var img = event.srcElement;
    img.src = "/images/web/hpage/errloader.jpg";
    img.onerror = null; //控制不要一直跳动
}
//img标签的onerror事件-加载不了图片使用默认的替换图片进行显示
function nofindRoll() {
    var img = event.srcElement;
    img.src = "http://mallmanage.51ipc.com//files/imgs/20161216/201612160202465956roll.jpg";
    img.onerror = null; //控制不要一直跳动
}
//获取提交购物车订单显示页列表（购买数量，购物车ID，商品名称，商品照片，商品价格，商品原价，商品描述属性,商品ID）
function GetUserCollectionDrink(cartNumber,cart_Id, goodsName, goodsPhoto, goodsPrice, goodsMarketprice, goodsState, goodsID) {
    var listLi = "<li id=\"liCollectionDrinkList_" + cart_Id + "\" class=\"my-border\"><a id=\"btnToShowPage_" + cart_Id + "\" onclick=\"btnToShowPage_Click(" + cart_Id + "," + goodsID + ")\">";
    listLi += "<table class=\"cart-confirm-table\"><tr><td rowspan=\"3\" class=\"cart-confirm-photo\">";
    listLi += "<img alt=\"pic\" src=\"" + goodsPhoto + "\" /></td><td class=\"cart-confirm-name\">";
    listLi += goodsName + "</td></tr><tr><td class=\"cart-confirmNumber-table\"><span >数量:" + cartNumber + "</span></td></tr><td class=\"cart-confirm-price\">";
    listLi += "&yen;<em id=\"emDrinkPrice_" + cart_Id + "\" style=\"font-style:normal\">" + goodsPrice + "</em><span><strong>&yen;" + goodsMarketprice + "</strong></span><b>〖总价：<b id=\"bSubtotalPrice_" + cart_Id + "\">"+(Number(goodsPrice) * Number(cartNumber)).toFixed(2)+"</b>〗</b></td>";
    listLi += "</tr></table></a></li>";
    return listLi;
}
//提交购物车订单显示默认地址
function GetUserCollectionAddress(userName,phone,address) {
    var listLi ="<table><tr><td></td><td><b>"+userName+"</b>&nbsp &nbsp   <span>"+phone+"</span></td></tr>";
    listLi += "<tr><td><span><img src=\"/images/web/icon_map.png \" /></span></td><td><span>" + address + "</span></td></tr></table>"
    return listLi;
}
//用户的收获地址管理
function GetUserAddressList(UserName, Mobile, Country, Province, City, District, Address,AddressID) {
    var listLi = "<li ><table style=\"width:100%\"><tr style=\"width:100%\"> <td style=\"width:80%\" class=\"border-td\" id=li_" + AddressID + " mydata=\"" + AddressID + "\" onclick=\"BtnTrunToUserCart(this)\"> <table class=\"border-tr\"> <tr> <td><b>" + UserName + "</b>&nbsp &nbsp  &nbsp  &nbsp <span>" + Mobile + "</span></td></tr>";
    listLi += " <tr><td>" + Country + Province + City + District + Address + "</td></tr></table> </td >  <td class=\"jump-btn\" style=\"width:20%;\" onclick=\"AddressEditor(this)\" mydata=\""+AddressID+"\"> <img id=\"edite_" + AddressID + "\" src=\"/images/icons-png/edite.png\"  class=\"icon-edite\"/></td></tr></table></li>";
    return listLi;
}
//跳转到编辑地址
function AddressEditor(obj) {
    var id = $.trim($(obj).attr("mydata"));
    $.mobile.changePage("/web/user_address_editor.aspx?id="+id+"", { transition: "slidefade" });
    //return false;
}
//挑选收货地址
function BtnTrunToUserCart(obj) {
    var currentPage = $("[id=divAddressPage]:last");
    var sysID = $.trim($(obj).attr("mydata"));
    if (sysID.length > 0) {
        $.cookie("defaultAddress", sysID);
        window.history.back();
    } else {
        ShowPromptBar(currentPage, "系统忙，请稍后再试");
    }
    //var url = "/web/ajax.aspx";
    //var args = { "jsonType": "setFirstUserAddress","id":sysID, "createTime": new Date() };
    //$.post(url, args, function (data) {
    //    if (data != "UNLOGIN") {
    //        if (data == "SUCCESS") {
    //            //在转向之前先生成COOKIE
    //            $.cookie("defaultAddress", sysID);
    //            $.mobile.changePage("/web/user_cart.aspx", { transition: "slidefade" });
    //        } else {
    //            ShowPromptBar(currentPage, data);
    //        }
    //    } else {
    //        $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
    //    }
    //});
}
//请填写收货地址
function GetUserCollectionData(data) {
    var listLi = "<table><tr style=\"padding-left:10px; font-family:SimSun;font-weight:200 ;\"><td rowspan=\"3\"><span> " + data + "</span></td><td><b></b>&nbsp &nbsp  &nbsp  &nbsp <span></span></td></tr>";
    listLi += "<tr><td></td><td></td></tr> </table>"
    return listLi;
}
//载入省市区地区信息
function LoadProvince(obj, provinceName, cityName, areaName) {
    var currentPage = $("[id=divAddressEditorPage]:last");
    var optionProvince = "";
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "choseChildAddress", "id": "", "createTime": new Date() };
    $.post(url, args, function (data) {
        var jsonData = eval(data);
        if (jsonData != "") {
            $.each(jsonData, function (key, val) {
                if (provinceName != "") {
                    if (val["RegionID"] == provinceName) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                }
            });
        } 
        currentPage.find("#ddlUserProvince").empty();
        currentPage.find("#ddlUserProvince").append(optionProvince);
        currentPage.find("#ddlUserProvince").selectmenu("refresh");
        LoadCity(currentPage, currentPage.find("#ddlUserProvince").val(), cityName, areaName);
    });
}
function LoadCity(obj, provinceId, cityName, areaName) {
    var currentPage = $("[id=divAddressEditorPage]:last");
    var optionProvince = "";
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "choseChildAddress", "id": provinceId, "createTime": new Date() };
    $.post(url, args, function (data) {
        var jsonData = eval(data);
        if (jsonData != "") {
            $.each(jsonData, function (key, val) {
                if (cityName != "") {
                    if (val["RegionID"] == cityName) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                }
            });
        } else {
            optionCity += "<option value=\"-1\">无</option>";
        }
        $(obj).find("#ddlUserCity").empty();
        $(obj).find("#ddlUserCity").append(optionProvince);
        currentPage.find("#ddlUserCity").selectmenu("refresh");
        CityChange(obj, provinceId, $(obj).find("#ddlUserCity").val(), areaName);
    });
}
function ProvinceChange(obj, provinceId, cityName) {
    var currentPage = $("[id=divAddressEditorPage]:last");
    var optionProvince = "";
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "choseChildAddress", "id": provinceId, "createTime": new Date() };
    $.post(url, args, function (data) {
        var jsonData = eval(data);
        if (jsonData != "") {
            $.each(jsonData, function (key, val) {
                if (cityName != "") {
                    if (val["RegionID"] == cityName) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionProvince += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                }
            });
        } else {
            optionCity += "<option value=\"-1\">无</option>";
        }
        $(obj).find("#ddlUserCity").empty();
        $(obj).find("#ddlUserCity").append(optionProvince);
        currentPage.find("#ddlUserCity").selectmenu("refresh");
        CityChange(obj, provinceId, $(obj).find("#ddlUserCity").val());
        
    });
}
function CityChange(obj, provinceId, cityId, areaName) {
    var currentPage = $("[id=divAddressEditorPage]:last");
    var optionArea = "";
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "choseChildAddress", "id": cityId, "createTime": new Date() };
    $.post(url, args, function (data) {
        var jsonData = eval(data);
        if (jsonData != "") {
            $.each(jsonData, function (key, val) {
                if (areaName != "") {
                    if (val["RegionID"] == areaName) {
                        optionArea += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionArea += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                } else {
                    if (key == 0) {
                        optionArea += "<option value=\"" + val["RegionID"] + "\" selected>" + val["RegionName"] + "</option>";
                    } else {
                        optionArea += "<option value=\"" + val["RegionID"] + "\">" + val["RegionName"] + "</option>";
                    }
                }
            });
        } else {
            optionCity += "<option value=\"-1\">无</option>";
        }
        $(obj).find("#ddlUserDistrict").empty();
        $(obj).find("#ddlUserDistrict").append(optionArea);
        currentPage.find("#ddlUserDistrict").selectmenu("refresh");
    });
}
//购物车勾选商品
function CheckThisBox(obj) {
    var currentPage = $("[id=divCartList]:last");
    var cartTotalPrice = 0;
    if ($(obj).find("[name=layout]").eq(0).is(':checked')) {
        $(obj).find("[name=layout]").eq(0).removeAttr("checked");
    } else {
        $(obj).find("[name=layout]").eq(0).prop("checked", "checked");
    }
    currentPage.find("[name=layout]").each(function () {//重新算价格
        if ($(this).eq(0).is(':checked')) {
            cartTotalPrice += Number($.trim($(this).parents("li").eq(0).find("[id^=bSubtotalPrice_]").eq(0).text()));
        }
    });
    currentPage.find("#emCartTotalPrice").text(cartTotalPrice.toFixed(2));
}
//生成购物车产品列表（购买数量，购物车ID，商品名称，商品照片，商品价格，商品原价，商品描述属性,商品ID）
function GetUserCartGoods(cartNumber, cart_Id, goodsName, goodsPhoto, goodsPrice, goodsMarketprice, goodsState, goodsID, isDelete, isEnable, goodsNumber, goodsQuotaNumber) {
    var listLi = "<li name=\"cart-li\" id=\"liCartGoodsList_" + cart_Id + "\" disabled=\"true\" ><div class=\"cart-div-box\" id=\"swipeleft-li\" >";
    listLi += "<div class=\"opt\" ";
    if (isDelete != 1 && isEnable != 0 && goodsNumber!=0) {
       listLi += " onclick=\"CheckThisBox(this);return false;\" ";
    }
    listLi += " ><input class=\"magic-checkbox\" type=\"checkbox\" name=\"layout\" id=\"c1" + cart_Id + "\" value=\"" + cart_Id + "\" ";
    if (isDelete != 1 && isEnable != 0 && goodsNumber!=0) {
    } else {
       listLi += " disabled=\"disabled\" ";
    }
    listLi += " /><label for=\"c1" + cart_Id + "\"></label></div>";
    listLi += "<a id=\"btnToShowPage_" + cart_Id + "\" ";
    listLi += ">";
    listLi += "<table class=\"cart-drink-table\"><tr";
    if (isDelete != 1 && isEnable != 0 && goodsNumber != 0) {
        listLi += " onclick=\"btnToShowPage_Click('" + cart_Id + "','" + goodsID + "')\" ";
    }
    listLi += "><td rowspan=\"3\" class=\"cart-drink-photo\">";
    listLi += "<img alt=\"pic\" src=\"" + goodsPhoto + "\" /></td><td class=\"cart-drink-name\">";
    listLi += goodsName + "</td></tr>";
    listLi += "<tr><td class=\"cart-drink-price\">";
    listLi += "&yen;<em id=\"emGoodsPrice_" + cart_Id + "\" style=\"font-style:normal\">" + goodsPrice + "</em><span><strong>&yen;" + goodsMarketprice + "</strong></span><b>〖已售：" + goodsState + "〗</b></td>";
    if (isDelete != 1 && isEnable != 0 && goodsNumber!=0) {
        listLi += "</tr><tr><td class=\"cart-drink-number\"><button id=\"btnReduce_" + cart_Id + "\" data-role=\"none\" class=\"btnReduce\" onclick=\"btnChangeCartNumber_Click('goods', '" + cart_Id + "', 'reduce')\">-</button>";
        listLi += "<input type=\"number\" id=\"tbNumber_" + cart_Id + "\" data-role=\"none\" class=\"tbNumber\" mydata=\""+cart_Id+"\" value=\"" + cartNumber + "\" onblur=\"ChangeCartNumber(this)\" />";
        listLi += "<button id=\"btnAdd_" + cart_Id + "\" data-role=\"none\" class=\"btnAdd\" onclick=\"btnChangeCartNumber_Click('goods', '" + cart_Id + "', 'add')\">+</button>";
        listLi += "<span>小计:&yen;<b id=\"bSubtotalPrice_" + cart_Id + "\">" + (Number(goodsPrice) * Number(cartNumber)).toFixed(2) + "</b></span>";
        if (goodsQuotaNumber > 0) {
            listLi += "<span>限购:<b>"+goodsQuotaNumber+"</b>件</span>";
        }
        listLi += "</td>";
    } else {
        if (isDelete == 1 && isEnable == 0) {
            listLi += "</tr><tr><td class=\"cart-drink-number\"><span style=\"color:#f2f2f2\">商品已下架</span></td>";
        } else {
            listLi += "</tr><tr><td class=\"cart-drink-number\"><span style=\"color:#f2f2f2\">商品已售罄</span></td>";
        }
    }
    listLi += "</tr></table>";
    listLi += "</a>";
    listLi += "</div><div class=\"btn\" onclick=\"btnDeleteCartOk_Click('" + cart_Id + "')\"><img src=\"/images/web/hpage/trashcan.png\" /></div></li>";
    return listLi;
}
//查看订单列表
function GetUserOrder(OrderSn, Consignee, PayName, Mobile, NOrderAddress, PayTime, AddTime, NOrderStatus, OrderStatus, NPayStatus, PayStatus, NShippingStatus, OrderAmount, OrderID, NSuppliers, ShippingStatus, FareMoeny, Logistical, LogisticalNumber) {

    var listLi = "<div class=\"orderlist_css\" name=\"ulUserOrderList\"><div id=\"liOrder_" + OrderSn + "_top\">ID：" + OrderSn + "<span mydata=\"" + OrderSn + "\" class=\"orderlist-show\" onclick=\"OrderShowMoreInfo(this)\">" + NOrderStatus + "&nbsp<strong><img src=\"../../images/web/hpage/add.png\" /></strong></span></div>";
    listLi += "<div id=\"liOrder_bottom_" + OrderSn + "\"><div class=\"order-table-div\">";
    listLi += "<div id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">收货姓名：</span><span class=\"order-right-div\">" + Consignee + "</span></div>";
    listLi += "<div id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">联系方式：</span><span class=\"order-right-div\">" + Mobile + "</span></div>";
    listLi += "<div ><span class=\"order-left-div\">商品货源：</span><span class=\"order-right-div\">" + NSuppliers + "</span></div>";
    listLi += "<div id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">收货地址：</span><span class=\"order-right-div\" >" + NOrderAddress + "</span></div>";
    listLi += "<div ><span class=\"order-left-div\">订单状态：</span><span class=\"order-right-div\">" + NOrderStatus + "</span></div>";
    listLi += "<div ><span class=\"order-left-div\">支付状态：</span><span class=\"order-right-div\">" + NPayStatus + "</span></div>";
    listLi += "<div ><span class=\"order-left-div\">物流状态：</span><span class=\"order-right-div\">" + NShippingStatus + "</span><span class=\"order-right-div-two\" onclick=\"CheckLogistics('" + Logistical + "','" + LogisticalNumber + "')\">查看物流信息</span></div>";
    listLi += "<div id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">下单时间：</span><span class=\"order-right-div\">" + AddTime + "</span></div>";
    if (PayStatus == "0") {
        listLi += "<div><span class=\"order-account\">合计：<em style=\"font-style:normal\">&yen;" + OrderAmount + "</em> (含运费 &yen"+FareMoeny+")</span></div>";
        if (OrderStatus == "2") {
            listLi += "<div><span class=\"order-last\" style=\"color:red\">订单已关闭</span></div>";
        } else {
            listLi += "<div><span class=\"order-last\"><a href=\"#\" class=\"btn-order-left\" onclick=\"BtnCancelOrder_Click('" + OrderSn + "','" + OrderID + "')\">取消订单</a>";
            listLi += "<a href=\"/web/payment/pay_center.aspx?OrderSn=" + OrderSn + "\" data-ajax=\"false\" class=\"btn-order-right\" data-transition=\"slide\">去支付</a></span></div>";
        }
    } else if (PayStatus == "3" || PayStatus == "9") {
        listLi += "<div  id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">支付方式：</span><span class=\"order-right-div\">" + PayName + "</span></div>";
        listLi += "<div  id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">付款时间：</span><span class=\"order-right-div\">" + PayTime + "</span></div>";
        listLi += "<div><span class=\"order-account\">合计：<em style=\"font-style:normal\">&yen;" + OrderAmount + "</em>(含运费 &yen" + FareMoeny + ")</span></div>";
        listLi += "<div><span class=\"order-last\" style=\"color:red\"><a href=\"tel:0574-83888011\" class=\"btn-order-right\" >联系客服</a></span></div>";
    } else if (PayStatus == "4") {
        listLi += "<div><span class=\"order-account\">合计：<em style=\"font-style:normal\">&yen;" + OrderAmount + "</em> (含运费 &yen" + FareMoeny + ")</span></div>";
        listLi += "<div><span class=\"order-last\" style=\"color:red\">订单已关闭";
        listLi += "</span></div>";
    } else if (PayStatus == "1" || PayStatus == "2") {
        listLi += "<div  id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">支付方式：</span><span class=\"order-right-div\">" + PayName + "</span></div>";
        listLi += "<div  id=\"tableOrderShow_tr_capacity\" style=\"display: none;\"><span class=\"order-left-div\">付款时间：</span><span class=\"order-right-div\">" + PayTime + "</span></div>";
        listLi += "<div><span class=\"order-account\">合计：<em style=\"font-style:normal\">&yen;" + OrderAmount + "</em> (含运费 &yen" + FareMoeny + ")</span></div>";
        if (ShippingStatus == "0") {
            if (PayStatus == "1") { listLi += "<div><span class=\"order-last\" style=\"color:red\">等待付款审核</span></div>"; } else { listLi += "<div><span class=\"order-last\" style=\"color:red\">等待仓库发货</span></div>"; }
        } else if (ShippingStatus == "1") {
            listLi += "<div><span class=\"order-last\" style=\"color:red\"><a href=\"#\" class=\"btn-order-right\" onclick=\"ConfirmItems_Click('" + OrderID + "')\">确认收货</a></span></div>";
        } else if (ShippingStatus == "2") {
            listLi += "<div><span class=\"order-last\" style=\"color:red\">订单已完成</span></div>";
        } else if (ShippingStatus == "3" || ShippingStatus == "4") {
            listLi += "<div><span class=\"order-last\" style=\"color:red\"><a href=\"tel:0574-83888011\" class=\"btn-order-right\" >联系客服</a></span></div>";
        } else {
            listLi += "<div><span class=\"order-last\" style=\"color:red\">如有疑问?<a href=\"tel:0574-83888011\" class=\"btn-order-right\" >联系客服</a></span></div>";
        }
    }
    listLi += "</div></div></div>";
    return listLi;
}
//商品详情页下拉显示更多信息
function OrderShowMoreInfo(obj) {
    var currentPage = $("[id=divPageUserOrder]:last");
    var orderSn = $(obj).attr("mydata");
    if (currentPage.find("[id^=liOrder_bottom_" + orderSn + "]").find("[id^=tableOrderShow_tr_capacity]").css("display") == "none") {
        $(obj).find("img").eq(0).attr("src", "../../images/web/hpage/reduce.png");
        currentPage.find("[id^=liOrder_bottom_" + orderSn + "]").find("[id^=tableOrderShow_tr_capacity]").show();
    } else {
        $(obj).find("img").eq(0).attr("src","../../images/web/hpage/add.png");
        currentPage.find("[id^=liOrder_bottom_" + orderSn + "]").find("[id^=tableOrderShow_tr_capacity]").hide();
    }
}
//查看订单列表商品显示
function GetUserOrderGoods(OrderSn, GoodsID, GoodsSn, GoodsPrice, GoodsName, GoodsImg,  BuyNumber, GoodsAttr) {
    var listLi = "<div id=\"liOrder_" + OrderSn + "_drink\" style=\"background-color:#F5F5F5\"><a href=\"/web/goods_show.aspx?goodsid=" + GoodsID + "\" data-transition=\"slidefade\"><table class=\"order-table\"><tr>";
    listLi += "<td class=\"order-drink-photo\"><img alt=\"pic\" src=\"" + GoodsImg + "\" /></td>";
    listLi += "<td class=\"order-drink-name\">" + GoodsName + "<br /><span>『" + GoodsSn + "』";
    listLi += "</span><br/><span>数量：" + BuyNumber + " <i style=\"color:red;font-style:normal\">总价： &yen; " + (Number(BuyNumber) * Number(GoodsPrice)*10/10).toFixed(2) + "</i></span></td>";
    listLi += "</tr></table></a></div>";
    return listLi;
}
//查看物流信息
function CheckLogistics(name,number) {
    if (ValIsDefine(name) && ValIsDefine(number)) {
        $.mobile.changePage("/web/logistical_search.aspx?name=" +encodeURIComponent(name) +"&number="+number+"", { transition: "slidefade" });
    } else {
        ShowNoteLoader("暂时无法查询此物流信息");
    }
}

//取消关闭订单信息——1弹窗
function BtnCancelOrder_Click(orderSn, orderID) {
    var currentPage = $("[id=divPageUserOrder]:last");
    currentPage.find("#btnShowboxLeft").attr("onclick", "DeleteOrder_Click('" + orderSn + "','" + orderID + "')");
    ShowShowbox(currentPage, "failure", "确定取消订单吗？", "#6699cc", "是", "否", "#", "slidefade");
    return false;
}
//取消关闭订单信息--2
function DeleteOrder_Click(orderSn, orderID) {
    var currentPage = $("[id=divPageUserOrder]:last");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "cancelOrder", "id": orderID, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data == "SUCCESS") {
            $.mobile.changePage("/web/order/order_return.aspx?type=cancelOrder", { transition: "slidefade" }); //window.location.reload()//刷新当前页面. 
        } else {
            ShowPromptBar(currentPage, data);
        }
    });
}
//确认收货
function ConfirmItems_Click(orderID) {
    var currentPage = $("[id=divPageUserOrder]:last");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "confirmOrder", "id": orderID, "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data == "SUCCESS") {
            $.mobile.changePage("/web/order/order_return.aspx?type=dealsuccess", { transition: "slidefade" });
        } else {
            ShowPromptBar(currentPage, data);//提示错误信息
        }
    });
}
//验证码计时-1
function codeTimeCount(obj_1,obj_2) {
    var timeCount = 60;
    $(obj_1).attr("mydata", "1");
    var countTime = setInterval(function () {
        obj_2.html("短信已发送" + timeCount + "s 后可再次获取").removeAttr("hidden");//失败
        timeCount--;
        if (timeCount == 0) {
            clearInterval(countTime);
            $(obj_1).attr("mydata", "0");
            obj_2.attr("hidden", "hidden");
        }
    }, 1000);
}
//验证码倒计时—2
function codeTimeCountSelf(obj) {
    var timeCount = 60;
    var countTime = setInterval(function () {
        $(obj).html("(" + timeCount + "s)");
        timeCount --;
        if (timeCount == 0) {
            clearInterval(countTime);
            $(obj).attr("mydata", "0");
            $(obj).html("发送");
        }
    }, 1000);
}
//获取子类目列表
function GetSubCategory(categoryName, cateImg,cateID) {
    var listStr = "<li><a href=\"/web/goods_list.aspx?goodstype=" + cateID + "\" data-transition=\"slide\"><img src=\"" + cateImg + "\" /> <span>" + categoryName + "</span> </a> </li>"
    return listStr;
}
//判断变量是否为NULL,"" Undefind
function ValIsDefine(val) {
    if (typeof(val) == "undefined") {
        return false;
    }
    if (val == null) {
       return false
    }
    if (val == "") {
        return false;
    }
    return true;
}
//用户解绑手机
function UnwrapPhone() {
    var currentPage = $("[id=divUserHomePage]:last");
    var url = "/web/ajax.aspx";
    var args = { "jsonType": "unwrapPhone", "createTime": new Date() };
    $.post(url, args, function (data) {
        if (data != "UNLOGIN") {
            if (data == "SUCCESS") {
                ShowPromptBar(currentPage, "解绑成功");//提示错误信息
            } else {
                ShowPromptBar(currentPage, data);//提示错误信息
            }
        } else {
            $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
        }
    });
    return false;
}
//用户换绑手机
function ChangePhone() {
    var currentPage = $("[id=divChangePhonePage]:last");
    var newPhone = currentPage.find("#newPhone").val();
    if (newPhone.length <= 0) {
        ShowPromptBar(currentPage, "请输入正确的手机格式");//提示错误信息
    } else {
        var url = "/web/ajax.aspx";
        var args = { "jsonType": "changePhone","phone":newPhone, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    ShowPromptBar(currentPage, "换绑成功");//提示错误信息
                } else {
                    ShowPromptBar(currentPage, data);//提示错误信息
                }
            } else {
                $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
            }
        });
    }
}
//用户换绑手机
function ChangeNickName() {
    var currentPage = $("[id=divChangeNickNamePage]:last");
    var newNickName = currentPage.find("#newNickName").val();
    if (newNickName.length <= 0) {
        ShowPromptBar(currentPage, "请输入新的昵称");//提示错误信息
    } else {
        var url = "/web/ajax.aspx";
        var args = { "jsonType": "changeNickName", "name": newNickName, "createTime": new Date() };
        $.post(url, args, function (data) {
            if (data != "UNLOGIN") {
                if (data == "SUCCESS") {
                    $.mobile.changePage("/web/user_home.aspx", { transition: "slidefade" });
                } else {
                    ShowPromptBar(currentPage, data);//提示错误信息
                }
            } else {
                $.mobile.changePage("/web/user_login.aspx", { transition: "slidefade" });
            }
        });
    }
}
//搜索框功能实现
function Search(obj) {
    $(obj).find("#myform").bind("submit", function () {
        var searchName = $(this).find("input").eq(0).val();
        $.mobile.changePage("/web/goods_list.aspx?searchname=" + searchName, { transition: "slidefade" });
        return false;
    });
}
//判断身份证号码是否有效
function isCardNo(card) {
    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X  
    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (reg.test(card) === false) {
        return false;
    } else {
        return true;
    }
}