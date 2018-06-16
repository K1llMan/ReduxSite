<article class="<?php post_class(); ?> section" id="post-<?php the_ID(); ?>">
    <h2 class="entry-title"><a title="<?php printf( esc_attr__( 'Permalink to %s', 'compass' ), the_title_attribute( 'echo=0' ) ); ?>" href="<?php the_permalink(); ?>" rel="bookmark">
        <?php the_title(); ?>
    </a></h2>
    <img class="size-large" alt="" src="images/featured-image.jpg" />
    <section class="entry-meta">
        <p class="flow-text"><?php the_date(); echo ' at '; the_time(); ?> by <?php the_author(); ?></p>
    </section>

    <section class="entry-content"><?php the_content(); ?></section>

    <section class="entry-meta"><?php if ( count( get_the_category() ) ) : ?>
        <span class="cat-links">
            Categories: <?php 
                $cats = split( '\|', get_the_category_list( '|' ) );
                foreach($cats as $cat)
                    echo '<div class="chip">'. $cat .'</div>';
            ?>
        </span>
    <?php endif; ?></section>

</article>
<div class="divider"></div>