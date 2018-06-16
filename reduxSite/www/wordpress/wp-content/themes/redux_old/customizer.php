<?php
    /**
     * Добавляет страницу настройки темы в админку Вордпресса
     */
    /*add_action('admin_menu', function(){
        add_theme_page('Customize', 'Customize', 'edit_theme_options', 'customize.php');
    });
    */

    /**
     * Добавляет секции, параметры и элементы управления (контролы) на страницу настройки темы
     */
    add_action('customize_register', function($customizer){
        // Remove default controls
        $customizer->remove_control('background_color');
        
        $customizer->add_section(
            'header',
            array(
                'title' => 'Header',
                'description' => 'Example section',
                'priority' => 35,
            )
        );
        
        /*
         * Header
         */
        /*
        $customizer->add_setting('header_image');
        $customizer->add_control(
            new WP_Customize_Image_Control(
                $customizer,
                'header_image_uploader',
                array(
                    'label' => 'Select header image',
                    'section' => 'header',
                    'settings' => 'header_image'
                )
            )
        );
        */
        
        /*
         * Main params
         */
        $customizer->add_setting('copyright_text');
        $customizer->add_control(
            'copyright_textbox',
            array(
                'label' => 'Copyright text',
                'section' => 'title_tagline',
                'settings' => 'copyright_text',
                'type' => 'text',
            )
        );

        $customizer->add_setting('is_hide_slides', array( 'default' => false ));
        $customizer->add_control(
            'hide_slides',
            array(
                'type' => 'checkbox',
                'label' => 'Hide slides',
                'section' => 'title_tagline',
                'settings' => 'is_hide_slides',
            )
        );

        $customizer->add_setting('main_page_posts_count', array( 'default' => 5 ));
        $customizer->add_control(
            'posts_num',
            array(
                'label' => 'Max main page posts',
                'section' => 'title_tagline',
                'settings' => 'main_page_posts_count',
                'type' => 'range',
                'input_attrs' => array(
                    'min' => 1,
                    'max' => 10,
                ),
            )
        );
        
        /*
        $customizer->add_setting('slides_count', array( 'default' => 2 ));
        $customizer->add_control(
            'slides_num',
            array(
                'label' => 'Slides count',
                'section' => 'title_tagline',
                'settings' => 'slides_count',
                'type' => 'range',
                'input_attrs' => array(
                    'min' => 2,
                    'max' => 10,
                ),
            )
        );
        */
        
        /*
         * Colors
         */
        // Theme
        $customizer->add_setting(
            'theme',
            array('default' => 'light')
        );
        
        $customizer->add_control(
            'theme',
            array(
                'type' => 'select',
                'label' => 'Theme',
                'section' => 'colors',
                'choices' => array(
                    'light' => 'Light',
                    'dark' => 'Dark',
                )
            )
        );         
        
        $accent_colors = array(
                    'red' => 'Red',
                    'pink' => 'Pink',
                    'purple' => 'Purple',
                    'deep-purple' => 'Deep Purple',
                    'indigo' => 'Indigo',
                    'blue' => 'Blue',
                    'light-blue' => 'Light Blue',
                    'cyan' => 'Cyan',
                    'teal' => 'Teal',
                    'green' => 'Green',
                    'light-green' => 'Light Green',
                    'lime' => 'Lime',
                    'yellow' => 'Yellow',
                    'amber' => 'Amber',
                    'orange' => 'Orange',
                    'deep-orange' => 'Deep Orange',
                );        
        
        $colors = array_merge($accent_colors, array(
                    'brown' => 'Brown',
                    'grey' => 'Grey',
                    'blue-grey' => 'Blue Grey',
                    'black' => 'Black',
                    'white' => 'White'
                ));
        
        // Primary theme color
        $customizer->add_setting(
            'primary_color',
            array('default' => 'blue')
        );        
        
        $customizer->add_control(
            'primary_color',
            array(
                'type' => 'select',
                'label' => 'Primary color',
                'section' => 'colors',
                'choices' => $colors,
            )
        );
        
        // Secondary theme color
        $customizer->add_setting(
            'secondary_color',
            array('default' => 'grey')
        );
        
        $customizer->add_control(
            'secondary_color',
            array(
                'type' => 'select',
                'label' => 'Secondary color',
                'section' => 'colors',
                'choices' => $colors,
            )
        );
        
        // Accent theme color
        $customizer->add_setting(
            'accent_color',
            array('default' => 'teal')
        );
        
        $customizer->add_control(
            'accent_color',
            array(
                'type' => 'select',
                'label' => 'Accent color',
                'section' => 'colors',
                'choices' => $accent_colors,
            )
        );
        /*
        $tones = array(
                    'accent-1' => '1',
                    'accent-2' => '2',
                    'accent-3' => '3',
                    'accent-4' => '4',
                ); 
        
        
        $customizer->add_setting(
            'accent_tone',
            array('default' => '4')
        );
        
        $customizer->add_control(
            'accent_tone',
            array(
                'type' => 'select',
                'label' => 'Tone',
                'section' => 'example_section_one',
                'choices' => $tones,
            )
        );  */      
        
        
        // ZiV DB params
        $customizer->add_section(
            'ZiVDB',
            array(
                'title' => 'ZiV DB',
                'description' => 'ZiV DB controls',
                'priority' => 35,
            )
        );
        /*
        $customizer->add_setting(
            'update_data'
        );
        
        $customizer->add_control(
            'update_data_button',
            array(
                'type' => 'button',
                'label' => 'Update ZiV DB',
                'section' => 'ZiVDB'
            )
        );
        */
    });
    
    // Return element color related to theme
    function GetThemeClass( $element = 'card' )
    {
        switch( $element )
        {
            case 'background':
                return get_theme_mod('theme') == 'light' ? 'grey lighten-5' : 'dark-background';
            case 'card':
                return get_theme_mod('theme') == 'light' ? 'white' : 'grey darken-3';
            case 'divider':
                return 'divider-opacity '. (get_theme_mod('theme') == 'light' ? 'black' : 'white');
            case 'text':
                return get_theme_mod('theme') == 'light' ? 'black-text' : 'white-text';
        }
    };
    
    // Localize scripts to attach css classes in js
    function theme_scripts() {
        // Register the script first.
        wp_register_script( 'document', 'js/document.js' );

        // Now we can localize the script with our data.
        $color_array = array( 
            'background' => GetThemeClass('background'), 
            'card' => GetThemeClass('card'),
            'divider' => GetThemeClass('divider'),
            'text' => GetThemeClass('text'),
            'accentColor' => get_theme_mod('accent_color') /*.' '. get_theme_mod('accent_tone'),
            'accentColorText' => 'text-'. get_theme_mod('accent_color') .' text-'. get_theme_mod('accent_tone')*/,
        );
        wp_localize_script( 'document', 'themeColors', $color_array );

        // The script can be enqueued now or later.
        wp_enqueue_script( 'document' );
    }
   
    add_action('wp_enqueue_scripts', 'theme_scripts');     
 ?>