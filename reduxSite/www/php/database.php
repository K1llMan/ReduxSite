 <?php
     // Database connection
    function ConnectToDB()
    {
        // Host/User/Password/Database
        //$link = mysqli_connect ('ec2-52-59-238-84.eu-central-1.compute.amazonaws.com', 'ReduxUser', 'ReduxUser', "Redux");
        $link = mysqli_connect ('localhost', 'root', '', 'redux');

        return $link;
    }

    // Format each element in string array
    function FormatArray( &$item1, $key, $formatStr ) 
    { 
        $item1 = sprintf($formatStr, $item1); 
    }

    // Make rows array
    function ToRowsArray( $rows )
    {
        $result = Array();
        while($row = mysqli_fetch_assoc($rows))
            $result[] = $row;

        return $result;
    }

    // Make JSON string with query result
    function JSONSerialize( $struct )
    {
        return json_encode($struct);
    }
?>