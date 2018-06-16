<?php
/**
 * Custom functions that act independently of the theme templates
 *
 * Eventually, some of the functionality here could be replaced by core features
 *
 * @package Intemporel
 * @author Deepak Bansal
 * @link http://www.dbansal.com
 */

/**
 * Adds custom classes to the array of body classes.
 *
 * @param array $classes Classes for the body element.
 * @return array
 */
function intemporel_body_classes( $classes ) {
	// Adds a class of group-blog to blogs with more than 1 published author.
	if ( is_multi_author() ) {
		$classes[] = 'group-blog';
	}

	return $classes;
}
add_filter( 'body_class', 'intemporel_body_classes' );

if ( version_compare( $GLOBALS['wp_version'], '4.1', '<' ) ) :
	/**
	 * Filters wp_title to print a neat <title> tag based on what is being viewed.
	 *
	 * @param string $title Default title text for current view.
	 * @param string $sep Optional separator.
	 * @return string The filtered title.
	 */
	function intemporel_wp_title( $title, $sep ) {
		if ( is_feed() ) {
			return $title;
		}

		global $page, $paged;

		// Add the blog name.
		$title .= get_bloginfo( 'name', 'display' );

		// Add the blog description for the home/front page.
		$site_description = get_bloginfo( 'description', 'display' );
		if ( $site_description && ( is_home() || is_front_page() ) ) {
			$title .= " $sep $site_description";
		}

		// Add a page number if necessary.
		if ( ( $paged >= 2 || $page >= 2 ) && ! is_404() ) {
			$title .= " $sep " . sprintf( esc_html__( 'Page %s', 'intemporel' ), max( $paged, $page ) );
		}

		return $title;
	}
	add_filter( 'wp_title', 'intemporel_wp_title', 10, 2 );

	/**
	 * Title shim for sites older than WordPress 4.1.
	 *
	 * @link https://make.wordpress.org/core/2014/10/29/title-tags-in-4-1/
	 * @todo Remove this function when WordPress 4.3 is released.
	 */
	function intemporel_render_title() {
		?>
		<title><?php wp_title( '|', true, 'right' ); ?></title>
		<?php
	}
	add_action( 'wp_head', 'intemporel_render_title' );
endif;

/**
 * Calls Google fonts
 *
 * Enqueued in functions.php
 */

function intemporel_fonts() {
    $fonts_url = '';
	$font_families = array();
	$font_families[] = 'Roboto:100,300,400,400italic,500,700';

	$query_args = array(
		'family' => urlencode( implode( '|', $font_families ) ),
		'subset' => urlencode( 'latin,latin-ext' ),
	);

	$fonts_url = add_query_arg( $query_args, '//fonts.googleapis.com/css' );

    return $fonts_url;
}

/**
 * Excerpt till first period (.)
 * Will be overridden if an excerpt defined by post authors
 *
 * @param Default Excerpt - First 55 words
 * @return Content till first period
 */

function intemporel_excerpt ($excerpt) {
	global $post;
	if( !has_excerpt($post->ID) ) {
		$dot = '.';
		$position = stripos($excerpt, $dot);
		if($position) {
			$offset = $position;
			return substr($excerpt,0,stripos($excerpt, $dot, $offset)) . '.';
		}
	} else {
		return $excerpt;
	}
}
add_filter( 'the_excerpt', 'intemporel_excerpt' );

/**
 * Adds a color scheme to admin area similar to the theme's styles
 *
 * Users can change their color schemes in Users > Your Profile
 */

function intemporel_color_scheme() {
	wp_admin_css_color(
		'intemporel', __( 'Intemporel', 'intemporel' ),
		get_stylesheet_directory_uri() . '/admin-color-schemes/intemporel/colors.css',
		array( '#303f9f', '#3F51B5', '#ff4081', '#f50057' )
	);
}
add_action( 'admin_init', 'intemporel_color_scheme');

/**
 * Post Class Filter
 *
 * Adds extra classes to certain templates
 */

function intemporel_post_class( $classes ) {

	global $post;
	$template = get_page_template_slug( $post->ID );

	if( ! is_singular() ) {
		$classes[] = 'excerpt';
	}

	if( $template == 'template-parts/content-poster' ) {
		$classes[] = 'poster';
	}

	return $classes;
}

add_filter( 'post_class', 'intemporel_post_class' );