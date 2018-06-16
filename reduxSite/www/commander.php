 <?php
    // Main module to handle outside commands
    include_once("php/commands.php"); 

    echo 'adfgadgsdfg';
    // Don't do it like this shit
    $authKey = "3mzyNGkbMUvWugUyNMV9";

    $commandParams = json_decode($_POST["CommandParams"]);
    if ($commandParams->AuthKey != $authKey)
    {
        echo "Received bad request.";
        exit;
    }

    switch ($commandParams->Command)
    {
        case "LoadPlayersMessages":
            GetPlayersMessages( $commandParams->SteamIDs );
            break;

        case "SendPlayerMessage":
            SendPlayerMessage( $commandParams->Data );
            break;

        case "MarkMessageRead":
            MarkMessageRead( $commandParams->MessageID );
            break;

        case "RecordPlayerSC":
            RecordPlayerSC( $commandParams->Data );
            break;

        case "LoadPlayerSC":
            LoadPlayerSC( $commandParams->SteamID );
            break;

        case "SendPlayerBuild":
            SendPlayerBuild( $commandParams->Data );
            break;

        case "LoadPlayerAbilities":
            LoadPlayerAbilities( $commandParams->SteamID );
            break;

        case "LoadGlobalAbilitiesStat":
            LoadGlobalAbilitiesStat();
            break;
            
        case "LoadFavBuilds":
            LoadFavBuilds( $commandParams->SteamID );
            break;

        case "SaveFavBuilds":
            SaveFavBuilds( $commandParams->Data );
            break;
        
        case "Testing":
            Testing();
            break;
    }
?>