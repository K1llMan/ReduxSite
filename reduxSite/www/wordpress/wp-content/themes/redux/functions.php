<?php
	// Add profile fields
	function modify_contact_methods($profile_fields) {

		// Add new fields
		$profile_fields['steam_id'] = 'SteamID';

		return $profile_fields;
	}
	add_filter('user_contactmethods', 'modify_contact_methods');

	// Returns context
	function get_context() {
		$isLogged = is_user_logged_in();
		$modules = array(
            '#messages' => array(
                'displayName' => 'Messages',
                'script' => 'messages.js',
                'template' => 'messages.tmp'
            ),
        
            '#abilities' => array(
                'displayName' => 'Abilities',
                'script' => 'abilities.js',
                'template' => 'abilities.tmp'
            ),
            '#heroes' => array(
                'displayName' => 'Heroes',
                'script' => 'heroes.js',
                'template' => 'heroes.tmp'
        ));
                
        if ($isLogged)
            $modules['#builds'] = array(
                'displayName' => 'Builds',
                'script' => 'builds.js',
                'template' => 'builds.tmp'
            );
        
        $output = array(
        	'isLogged' => $isLogged,
        	'modules' => $modules
    	);

        echo json_encode( $output );
	    wp_die();
	}

	add_action( 'wp_ajax_nopriv_get_context', 'get_context' );
	add_action( 'wp_ajax_get_context', 'get_context' );

	// Messages table
	function get_messages_table() {
        global $wpdb;
        $result = $wpdb->get_results ( "SELECT * FROM redux_messages order by ID desc" );

        echo json_encode( $result);
	    wp_die();
	}

	add_action( 'wp_ajax_nopriv_get_messages_table', 'get_messages_table' );
	add_action( 'wp_ajax_get_messages_table', 'get_messages_table' );	    
    
	// Messages table
	function get_abilities() {
        global $wpdb;
        $result = $wpdb->get_results ( "SELECT * FROM redux_abilities order by PickCount desc" );

        echo json_encode( $result);
	    wp_die();
	}

	add_action( 'wp_ajax_nopriv_get_abilities', 'get_abilities' );
	add_action( 'wp_ajax_get_abilities', 'get_abilities' );	

	// Messages table
	function get_heroes() {
		global $wpdb;
        $result = $wpdb->get_results ( "SELECT * FROM redux_heroes order by PickCount desc" );

        echo json_encode( $result);
	    wp_die();
	}

	add_action( 'wp_ajax_nopriv_get_heroes', 'get_heroes' );
	add_action( 'wp_ajax_get_heroes', 'get_heroes' );	

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
    
	// Upload builds
	function download_builds() {
	    global $wpdb;
        $result = $wpdb->get_results ("select * from redux_builds");
        
        $output = array();
        for($i = 0; $i < count($result); $i++) {
            $output[] = array(
                'id' => $result[$i]->ID,
                'title' => $result[$i]->Title,
                'hero' => $result[$i]->Hero,
                'attr' => $result[$i]->Attr,
                'desc' => $result[$i]->Description,
                'author' => $result[$i]->AuthorSteamID,
                'categories' => $result[$i]->Categories,
                'balanced' => $result[$i]->Balanced,
                'unbalanced' => $result[$i]->Unbalanced
            );
        }
        
        echo json_encode($output);
	    wp_die();
	}

	add_action( 'wp_ajax_download_builds', 'download_builds' );
    
	// Clear abilities
	function clear_abilities() {
	    global $wpdb;
        $result = $wpdb->get_results ("delete from redux_abilities");
        
        echo json_encode($output);
	    wp_die();
	}

	add_action( 'wp_ajax_clear_abilities', 'clear_abilities' );