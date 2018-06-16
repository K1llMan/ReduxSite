<?php
/**
 * The template part for displaying results in search pages.
 *
 * Learn more: http://codex.wordpress.org/Template_Hierarchy
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
			<?php the_excerpt(); ?>
		</div><!-- .entry-summary -->
	</div>
</article><!-- #post-## -->

