 <?php
    include_once("database.php"); 
    function ReturnError( $errConst )
    {
        switch ( $errConst ) 
        {
            case "dbConnectProblem":
                echo "Database connection fault.";
                break;
            
            default:
                # code...
                break;
        }
    }

    function printArray($array, $pad=''){
         foreach ($array as $key => $value){
            echo $pad . "$key => $value";
            if(is_array($value)){
                printArray($value, $pad.' ');
            }  
        } 
    }

    // Test function
    function Testing()
    {
        echo "Successful testing! Enjoy!";
    }

    // Get messages
    function GetPlayersMessages( $array )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        // Select ids to insert and format elements
        $query = 
            "select *" .
            " from redux_messages".
            " where SteamID in (". implode( ", ", $array) . ") and not Reply is null and not IsPlayerRead";

        $result = mysqli_query($link, $query);
        mysqli_close($link);

        echo JSONSerialize( ToRowsArray($result) );
    }

    // Get messages
    function SendPlayerMessage( $data )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        // Select ids to insert and format elements
        $query = 
            "insert into redux_messages (SteamID, Nickname, Comment, TimeStamp)".
            " values ({$data->SteamID}, '{$data->Nickname}', '{$data->Message}', {$data->TimeStamp})";

        $result = mysqli_query($link, $query);
        mysqli_close($link);
    }

    // Get messages
    function MarkMessageRead( $message_id )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        // Select ids to insert and format elements
        $query = 
            "update redux_messages".
            " set IsPlayerRead = true".
            " where ID = {$message_id}";

        $result = mysqli_query($link, $query);
        mysqli_close($link);
    }

    // Save settings
    function RecordPlayerSC( $data )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        // Select ids to insert and format elements
        $query = 
            "insert into redux_players (SteamID, Settings)". 
            " values ( {$data->SteamID}, '{$data->SettingsCode}')".
            " on duplicate key update".
            " Settings = '{$data->SettingsCode}'";

        $result = mysqli_query($link, $query);
        mysqli_close($link);
    }

    // Load settings
    function LoadPlayerSC( $steamID )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "select Settings".
            " from redux_players".
            " where SteamID = {$steamID}";

        $result = mysqli_query($link, $query);
        mysqli_close($link);

        echo JSONSerialize( ToRowsArray($result) );
    }

    // Save used abilities
    function SendPlayerBuild( $data ) 
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "select MostUsedSkills".
            " from redux_players".
            " where SteamID = {$data->SteamID}";

        $result = mysqli_query($link, $query);

        $row = array_shift(ToRowsArray( $result ));

        $abilities = json_decode( $row["MostUsedSkills"], true );
        if (is_null($abilities))
            $abilities = array();

        foreach (json_decode( $data->AbilityString) as $ability) {
            if (array_key_exists ($ability, $abilities))
                $abilities[$ability]++;
            else
                $abilities[$ability] = 1;

            // Update global stat
            $query = 
                "insert into redux_abilities (Name, PickCount)". 
                " values ( '{$ability}', 1)".
                " on duplicate key update".
                " PickCount = PickCount + 1";

            $result = mysqli_query($link, $query);
        }

        $storingData = json_encode($abilities);
        $query = 
            "insert into redux_players (SteamID, MostUsedSkills)". 
            " values ( {$data->SteamID}, '{$storingData}')".
            " on duplicate key update".
            " MostUsedSkills = '{$storingData}'";

        $result = mysqli_query($link, $query);
        mysqli_close($link);
    }

    // Get player abilities list
    function LoadPlayerAbilities( $data ) 
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "select MostUsedSkills".
            " from redux_players".
            " where SteamID = {$data->SteamID}";

        $result = mysqli_query($link, $query);       
        mysqli_close($link);

        echo JSONSerialize( ToRowsArray($result) );
    }

    // Send global abilities stats
    function LoadGlobalAbilitiesStat()
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "select Name, PickCount".
            " from redux_abilities".
            " order by PickCount desc";

        $result = mysqli_query($link, $query);       
        mysqli_close($link);

        echo JSONSerialize( ToRowsArray($result) );
    }
    
    // Load players favorite builds
    function LoadFavBuilds( $steamID )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "select FavBuilds".
            " from redux_players".
            " where SteamID = {$data->SteamID}";

        $result = mysqli_query($link, $query);       
        
        $row = mysqli_fetch_assoc($result);
        $buildNames = json_decode($row->FavBuilds);

        mysqli_close($link);

        var_dump($buildNames);
        //echo JSONSerialize( ToRowsArray($result) );    
    
    }
    
    function SaveFavBuilds( $data )
    {
        $link = ConnectToDB();
        if (!$link)
        {
            ReturnError( "dbConnectProblem" );
            return;
        }

        $query = 
            "insert into redux_players (SteamID, FavBuilds)". 
            " values ( {$data->SteamID}, '{$data->Builds}')".
            " on duplicate key update".
            " FavBuilds = '{$data->Builds}'";

        $result = mysqli_query($link, $query);       
        mysqli_close($link);
    }
?>