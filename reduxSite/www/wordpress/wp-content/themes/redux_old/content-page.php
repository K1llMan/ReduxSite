<article class="section" id="post-<?php the_ID(); ?>">
    <div class="col s12">

        <h2 class="page-title"><?php the_title(); ?></h2>

        <section class="entry-content"><?php the_content(); ?></section>
        
        <section class="entry-meta">
            <p class="flow-text right-align"><b><?php the_date(); echo ' at '; the_time(); ?></b></p>
        </section>
    </div>
</article>