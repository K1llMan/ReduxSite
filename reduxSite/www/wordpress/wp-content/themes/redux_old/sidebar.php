<!-- the sidebar - in WordPress this will be populated with widgets -->
<aside class="sidebar widget-area <?php echo is_active_sidebar( 'sidebar' ) ? 'section' : '' ?>" role="complementary">
    <?php if ( is_active_sidebar( 'sidebar' ) ) : ?>
    	<?php dynamic_sidebar( 'sidebar' ); ?>
    <?php endif; ?>
</aside>