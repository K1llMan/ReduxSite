<?php
/**
 * The template for displaying the footer.
 *
 * Contains the closing of the #content div and all content after
 *
 * @package Intemporel
 * @author Deepak Bansal
 * @link http://www.dbansal.com
 */

?>

	</div><!-- #content -->

	<footer id="colophon" class="site-footer" role="contentinfo">
		<div class="site-info">
			<span class="footer-message"><?php echo esc_attr( get_theme_mod( 'intemporel_footer_message' ) ); ?></span>
			<?php if ( get_theme_mod( 'intemporel_footer_message' ) ) { ?>
				<span class="sep"> | </span>
			<?php } ?>
			<?php printf( esc_html__( 'Built with %s', 'intemporel' ), '<a href="http://dbansal.com" title="Intemporel WordPress Theme">Intemporel</a>' ); ?>
			<span class="sep"> and </span>
			<a href="<?php echo esc_url( __( 'http://wordpress.org/', 'intemporel' ) ); ?>" title="WordPress"><?php printf( esc_html__( '%s', 'intemporel' ), 'WordPress' ); ?></a>
		</div><!-- .site-info -->

		<nav class="social-networks">
			<?php wp_nav_menu( array( 
					'theme_location' => 'social',
					'depth'          => 1,
					'link_before'    => '<span class="screen-reader-text">',
					'link_after'     => '</span>',
				) ); ?>
		</nav>
	</footer><!-- #colophon -->
</div><!-- #page -->

<?php wp_footer(); ?>

</body>
</html>
