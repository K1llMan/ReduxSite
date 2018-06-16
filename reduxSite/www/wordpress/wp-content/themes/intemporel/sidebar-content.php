<?php
/**
 * The sidebar containing the main widget area.
 *
 * @package Intemporel
 * @author Deepak Bansal
 * @link http://www.dbansal.com
 */

if ( ! is_active_sidebar( 'sidebar-2' ) ) {
	return;
}
?>

<div class="content-sidebar" role="complementary">
	<?php dynamic_sidebar( 'sidebar-2' ); ?>
</div><!-- #secondary -->