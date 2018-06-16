<?php
/**
 * Template part for displaying poster-post.
 *
 * @package Intemporel
 * @author Deepak Bansal
 * @link http://www.dbansal.com
 */

$thumb_id = get_post_thumbnail_id();

$thumb_url = wp_get_attachment_image_src($thumb_id,'full', true);

//$classes = array('excerpt', 'poster');

?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
<div class="poster">
	
	<div style="background:url(<?php echo esc_url($thumb_url[0]);?>) no-repeat fixed center;"  class="poster-bg"></div>
	<div class="poster-main">
		<div class="poster-content">
			<header class="entry-header">
				<?php if ( 'post' == get_post_type() ) : ?>
					<div class="entry-meta">
						<?php intemporel_posted_on(); ?>
					</div><!-- .entry-meta -->
				<?php endif; ?>
				<?php the_title( sprintf( '<h1 class="entry-title"><a href="%s" rel="bookmark">', esc_url( get_permalink() ) ), '</a></h1>' ); ?>
			</header><!-- .entry-header -->

			<div class="entry-summary">
				<?php
					/* translators: %s: Name of current post */
					the_excerpt( sprintf(
						wp_kses( __( 'Continue reading %s <span class="meta-nav">&rarr;</span>', 'intemporel' ), array( 'span' => array( 'class' => array() ) ) ),
						the_title( '<span class="screen-reader-text">"', '"</span>', false )
					) );
				?>
			</div><!-- .entry-summary -->
		</div><!-- .poster-content -->
	</div><!-- .poster-main -->
</div>
</article><!-- #post-## -->
