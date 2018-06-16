<?php get_header(); ?>

<div class="right"><?php wp_loginout(); ?></div>

<div class="row" id="content">
    <div class="col s12">
        <div class="col s12">
            <?php get_template_part( 'content', 'main' ); ?>
        </div>
    </div>
</div> <!-- #content-->
<?php get_footer(); ?>