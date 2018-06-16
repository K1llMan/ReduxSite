<?php
	// Add profile fields
	function modify_contact_methods($profile_fields) {

		// Add new fields
		$profile_fields['steam_id'] = 'SteamID';

		return $profile_fields;
	}
	add_filter('user_contactmethods', 'modify_contact_methods');

	// Add reply function
	function add_reply() {
		$id = $_POST['id'];
		$reply = urlencode($_POST['reply']);
		$steamID = get_user_meta( get_current_user_id(), 'steam_id', true ); 
		
	    global $wpdb;
	    $result = $wpdb->get_results ( 
	    	"update redux_messages" .
	    	" set Reply = '{$reply}', DeveloperSteamID = {$steamID}, IsPlayerRead = false" .
	    	" where ID = {$id}"
	    );

	    wp_die();
	}

	add_action( 'wp_ajax_add_reply', 'add_reply' );

	// Remove reply function
	function remove_reply() {
		$id = $_POST['id'];
		
	    global $wpdb;
	    $result = $wpdb->get_results ( 
	    	"delete from redux_messages" .
	    	" where ID = {$id}"
	    );

	    wp_die();
	}

	add_action( 'wp_ajax_remove_reply', 'remove_reply' );	


	// Remove reply function
	function remove_selected() {
		$list = implode( ", ", $_POST['id_list']);
		
	    global $wpdb;
	    $result = $wpdb->get_results ( 
	    	"delete from redux_messages" .
	    	" where ID in ({$list})"
	    );

	    wp_die();
	}

	add_action( 'wp_ajax_remove_selected', 'remove_selected' );

	// Upload builds
	function upload_builds() {
		$json = preg_replace('/\\\\/', '', $_POST['builds_data']);
		$builds = json_decode($json);

	    global $wpdb;

	    foreach ($builds as $build) {
		    $result = $wpdb->get_results ( 
				"insert into redux_builds (ID, Title, Hero, Attr, Description, AuthorSteamID, Categories, Balanced, Unbalanced)". 
	            " values ( \"{$build->id}\", \"{$build->title}\", \"{$build->hero}\", \"{$build->attr}\", \"{$build->desc}\", {$build->author}, \"{$build->categories}\"," . 
	            " \"{$build->balanced}\", \"{$build->unbalanced}\")".
	            " on duplicate key update".
	            " Title = \"{$build->title}\", Hero = \"{$build->hero}\", Attr = \"{$build->attr}\", Description = \"{$build->desc}\", AuthorSteamID = {$build->author}, " .
	            " Categories = \"{$build->categories}\", Balanced = \"{$build->balanced}\", Unbalanced = \"{$build->unbalanced}\""
	    	);
	    }

	    wp_die();
	}

	add_action( 'wp_ajax_upload_builds', 'upload_builds' );			