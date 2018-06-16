<article class="section" id="post-<?php the_ID(); ?>">
    <div class="col s12">

        <div class="content-title"><b><?php the_title(); ?></b></div>

        <section class="entry-content"><?php the_content(); ?></section>
        
        <section class="entry-meta">
            <p class="flow-text right-align"><b><?php the_date(); echo ' at '; the_time(); ?></b></p>
        </section>
    </div>
</article>