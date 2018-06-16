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
		<link type="text/css" rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ) ?>/assets/css/materialize.min.css"  media="screen,projection"/>
        <link type="text/css" rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ) ?>/assets/css/document.css"  media="screen,projection"/>
        <link type="text/css" rel="stylesheet" href="<?php bloginfo( 'stylesheet_directory' ) ?>/assets/css/styles.css"  media="screen,projection"/>

		<?php wp_head(); ?>		
    </head>

	<script>
		var templateDir = "<?php bloginfo('template_directory') ?>";
	</script>

	 <!--Import jQuery before materialize.js-->
	<script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/assets/js/jquery-2.2.1.min.js"></script>
	<script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/assets/js/materialize.min.js"></script>

    <script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/assets/js/templater.js"></script>    
    <script type="text/javascript" src="<?php bloginfo( 'stylesheet_directory' ); ?>/assets/js/script.js"></script>

    <body class="container">
        <div class="topBar row">
        	<div class="white col s12 z-depth-1">
        		<div class="right"><?php wp_loginout(); ?></div>
        	</div>
        </div>
     
        <div class="main row">