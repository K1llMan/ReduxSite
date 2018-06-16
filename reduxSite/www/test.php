<?php 
	$resource = mysqli_connect ('ec2-52-59-238-84.eu-central-1.compute.amazonaws.com', 'ReduxUser', 'ReduxUser', 'redux');
	if (!$resource) { 
		die ('Ошибка при подключении: ' . mysql_error ()); 
	} 
	echo 'Подключено успешно!'; 
	mysqli_close ($link); 
?>