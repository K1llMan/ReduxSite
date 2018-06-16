<div>
    <ul class="tabs">
        <li class="tab col s4"><a href="#Messages">Messages</a></li>
        <li class="tab col s4"><a href="#Abilities">Abilities</a></li>
        <li class="tab col s4"><a href="#Builds">Builds</a></li>        
    </ul>

    <div id="Builds">
    <?php if (is_user_logged_in())
        echo '<input type="file" id="file-input" />';
    ?>
    </div>

    <div id="Messages">
        <table class="highlight responsive-table">
            <thead>
                <tr>
                    <?php if (is_user_logged_in())
                        echo '<th>Selection</th>';
                    ?>
                     <th>Comment</th>
                     <th>From</th>
                     <th>Reply</th>
                </tr>
            </thead>
            <tbody>
                <?php
                    global $wpdb;
                    $result = $wpdb->get_results ( "SELECT * FROM redux_messages order by ID desc" );
                    foreach ( $result as $print )   {
                    ?>
                    <tr>
                        <?php if (is_user_logged_in())
                            echo 
                            '<td>
                                <p class="center">
                                  <input type="checkbox" id="check_'. $print->ID .'" />
                                  <label for="check_'. $print->ID .'"></label>
                                </p>
                            </td>';
                        ?>
                        <td><?php echo urldecode($print->Comment);?></td>
                        <td data-SteamID="<?php echo $print->SteamID;?>"><?php echo urldecode($print->Nickname);?></td>
                        <td <?php if (is_user_logged_in()) echo 'id="id_' . $print->ID . '"';?>><?php echo urldecode($print->Reply);?></td>
                    </tr>
                <?php }	?>
            </tbody>
        </table>
        
        <?php if (is_user_logged_in())
            echo 
            '<div class="fixed-action-btn horizontal" style="bottom: 25px; right: 24px;">
                <a class="btn-floating btn-large red tooltipped" data-position="top" data-delay="50" data-tooltip="Remove" id="removeSelectedMenu">
                  <i class="large material-icons">delete</i>
                </a>
             </div>';
        ?>        
    </div>

    <div id="Abilities">
        <?php
            global $wpdb;
            $result = $wpdb->get_results ( "SELECT * FROM redux_abilities order by PickCount desc" );
            
            function sumPicks($row)
            {
                return $row->PickCount + $row->BanCount; 
            }
            
            $sum = max ( array_map('sumPicks', $result) );
            foreach ( $result as $print )   { ?>
                <div class="ability-row flow-right">
                    <div class="ability-caption"> <?php echo $print->Name; ?> </div>
                    <div class="abilities-count flow-down">
                        <span class="pick"><?php echo $print->PickCount . '/' . $print->BanCount; ?></span> 
                        <span class="winrate"><?php echo $print->LossCount . '/' . $print->WinCount; ?></span>
                    </div>
                    <div class="flow-down bars">
                        <div class="pick-bar">
                            <div class="pick" style="width: <?php echo $print->PickCount / $sum * 100; ?>%;"></div>
                            <div class="ban" style="width: <?php echo $print->BanCount / $sum * 100; ?>%;"></div>
                        </div>
                        <div class="winrate-bar" style="width: <?php echo $print->PickCount / $sum * 100; ?>%;">
                            <div class="loss" style="width: <?php echo $print->LossCount / $print->PickCount * 100; ?>%;"></div>
                            <div class="win" style="width: <?php echo $print->WinCount / $print->PickCount * 100; ?>%;"></div>
                        </div>
                    </div>
                </div>
        <?php }	?>       
    </div>
</div>    

<?php if (is_user_logged_in())
	echo '<div class="hide" id="hiddenDiv">
		<div id="submitReply">
		    <label for="reply">Reply</label>
		    <textarea name="reply" id="textarea" class="materialize-textarea" length="1024"></textarea>
		    <button class="btn" id="sendButton">Send</button>
		    <button class="btn" id="cancelButton">Cancel</button>
		    <button class="btn" id="removeButton">Remove</button>
		</div>
	</div>';
?>
