/**
 * script.js
 *
 * Handles toggling the navigation menu for and search box.
 */
( function() {

	var menu = document.querySelector('.menu-toggle');
	var nav = document.getElementById('site-navigation');

	menu.onclick = function() {
		if ( -1 !== nav.className.indexOf( 'toggled' ) ) {
			nav.className = nav.className.replace( ' toggled', '' );
		} else {
			nav.className += ' toggled';
		}
	}

	var search = document.querySelector('.search-toggle');
	var form = document.querySelector('.search-form');
	var input = document.querySelector('.search-field');

	search.onclick = function() {
		if ( -1 !== form.className.indexOf( 'search-toggled' ) ) {
			form.className = form.className.replace( ' search-toggled', '' );
			search.innerHTML = '<span class="genericon genericon-search"></span>';
		} else {
			form.className += ' search-toggled';
			search.innerHTML = '<span class="genericon genericon-close-alt"></span>';
			input.focus();
		}
	}

	window.addEventListener("scroll", function(evt) {

        // This value is your scroll distance from the top
        var distance_from_top = document.body.scrollTop;

        var header = document.getElementById("masthead");
        var menu = header.querySelector(".menu-toggle");
        var search = header.querySelector(".search-toggle");
        var title = header.getElementsByTagName("h1")[0];
        var form = header.getElementsByTagName("form")[0];
        var content = document.getElementById("content");


        // The user has scrolled to the tippy top of the page.
        if(distance_from_top === 0){
            if ( -1 !== title.className.indexOf( 'onscroll-height' ) ) {
            	title.className = title.className.replace( ' onscroll-height', '' );
                menu.className = menu.className.replace( ' onscroll-height', '' );
                search.className = search.className.replace( ' onscroll-height', '' );
                if (window.innerWidth <= 980) {
                    form.setAttribute("style","top:.8em");
                    content.setAttribute("style","padding-top:60");
                } else {
                    form.setAttribute("style","top:2em");
                    content.setAttribute("style","padding-top:100px");
                }
            }
        }

        // The user has scrolled down the page.
        if(distance_from_top > 48){
        	if ( -1 !== title.className.indexOf( 'onscroll-height' ) ) {
                
            } else {
                title.className += ' onscroll-height';
                menu.className += ' onscroll-height';
                search.className += ' onscroll-height';
                form.setAttribute("style","top:.3em"); 
                content.setAttribute("style","padding-top:48px");
            }
        }
    });

} )();
