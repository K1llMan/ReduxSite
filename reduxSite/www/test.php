<?php 
	$resource = mysqli_connect ('ec2-52-59-238-84.eu-central-1.compute.amazonaws.com', 'ReduxUser', 'ReduxUser', 'redux');
	if (!$resource) { 
		die ('������ ��� �����������: ' . mysql_error ()); 
	} 
	echo '���������� �������!'; 
	mysqli_close ($link); 
?>