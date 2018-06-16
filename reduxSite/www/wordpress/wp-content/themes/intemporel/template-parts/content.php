<?php
/**
 * Template part for displaying posts.
 *
 * @package Intemporel
 * @author Deepak Bansal
 * @link http://www.dbansal.com
 */

?>

<article id="post-<?php the_ID(); ?>" <?php post_class(); ?>>
	<figure class="entry-thumbnail">
		<?php the_post_thumbnail( 'medium' ) ?>
	</figure>
	<div class="excerpt-content">
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

			<?php
				wp_link_pages( array(
					'before' => '<div class="page-links">' . esc_html__( 'Pages:', 'intemporel' ),
					'after'  => '</div>',
				) );
			?>
		</div><!-- .entry-content -->
	</div>
</article><!-- #post-## -->
