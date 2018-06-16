<html <?php language_attributes(); ?> class="no-js">
	<head>
    	<meta charset="<?php bloginfo( 'charset' ); ?>" />
	      <!--Let browser know website is optimized for mobile-->
	    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

	    <title>
		    <?php
				global $page, $paged;
				wp_title( '|', true, 'right' );
				// Add the blog name.
				bloginfo( 'name' );
				// Add the blog description for the home/front page.
				$site_description = get_bloginfo( 'description', 'display' );
				if ( $site_description && ( is_home() || is_front_page() ) )
				    echo " | $site_description";
			?>
		</title>

		<link href="<?php bloginfo( 'stylesheet_url' ); ?>" rel="stylesheet" media="all" type="text/css" />
		<!--Import Google Icon Font-->
		<link href="http://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">

		<!--Import materialize.css-->
		<link type="text/css" rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ) ?>/css/materialize.css"  media="screen,projection"/>
        <link type="text/css" rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ) ?>/css/document.css"  media="screen,projection"/>

		<?php wp_head(); ?>		
    </head>

	<script>
		var templateDir = "<?php bloginfo('template_directory') ?>";
	</script>

	 <!--Import jQuery before materialize.js-->
	<script type="text/javascript" src="https://code.jquery.com/jquery-2.2.1.min.js"></script>
	<script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/js/materialize.js"></script>
	<script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/js/document.js"></script>

    <body>
        <header>
        </header>
     
        <div class="main container">