<?php
/**
 * Основные параметры WordPress.
 *
 * Скрипт для создания wp-config.php использует этот файл в процессе
 * установки. Необязательно использовать веб-интерфейс, можно
 * скопировать файл в "wp-config.php" и заполнить значения вручную.
 *
 * Этот файл содержит следующие параметры:
 *
 * * Настройки MySQL
 * * Секретные ключи
 * * Префикс таблиц базы данных
 * * ABSPATH
 *
 * @link https://codex.wordpress.org/Editing_wp-config.php
 *
 * @package WordPress
 */

// ** Параметры MySQL: Эту информацию можно получить у вашего хостинг-провайдера ** //
/** Имя базы данных для WordPress */
define('DB_NAME', 'redux');

/** Имя пользователя MySQL */
define('DB_USER', 'root');

/** Пароль к базе данных MySQL */
define('DB_PASSWORD', '');

/** Имя сервера MySQL */
define('DB_HOST', 'localhost');

/** Кодировка базы данных для создания таблиц. */
define('DB_CHARSET', 'utf8');

/** Схема сопоставления. Не меняйте, если не уверены. */
define('DB_COLLATE', '');

/**#@+
 * Уникальные ключи и соли для аутентификации.
 *
 * Смените значение каждой константы на уникальную фразу.
 * Можно сгенерировать их с помощью {@link https://api.wordpress.org/secret-key/1.1/salt/ сервиса ключей на WordPress.org}
 * Можно изменить их, чтобы сделать существующие файлы cookies недействительными. Пользователям потребуется авторизоваться снова.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         '3C}@@2|Wm5NJ4HNBCxqZ.kHFXS}ify5eo1#+d7)lCs_g*XM7dT@-+B-R{{%%f)hT');
define('SECURE_AUTH_KEY',  ']ztm2C_GwD$9:-e4$thg<R6q*ky#/u5L+PASoR95i1miPP&=RTs|u8DnKQPCaK:i');
define('LOGGED_IN_KEY',    '-VyuX|[,%9R}MMPfH2b_! uil1b^Jd/=hV9*2f<62;[^-Z}GSLu[<_D:PM:`+5)(');
define('NONCE_KEY',        'FD|FYHN~XGo_AS$+WI*F!*O7E:19UEir/3K~R}t4u3@T0 m45<TQ8*wjex$<z Nr');
define('AUTH_SALT',        'JqSeZZr=DNvsJ<d6{3y;jZclYu)PyJa!,2tdr#?#$g#OWWnU1+F{N{]+@2mj>[Y1');
define('SECURE_AUTH_SALT', 'uV8+px6mMCP)+]T8nfr#!f6Ew-t|t8$-/ >v,kBR3DOgWEh(^8S[#4Y#+R|~N5Y0');
define('LOGGED_IN_SALT',   'xxzZjE0GZ1]T[fUFp<AF7z%`*g685ePpL,5Pk|++D~;_NBB.|03DdAvu[)3.SN>P');
define('NONCE_SALT',       'c(_H-GYCih+D|F%^*?X>+*6*xO)IgYU@@~PX~L[R0;|]QL`c|-ld>?D1GGy{I=C@');

/**#@-*/

/**
 * Префикс таблиц в базе данных WordPress.
 *
 * Можно установить несколько сайтов в одну базу данных, если использовать
 * разные префиксы. Пожалуйста, указывайте только цифры, буквы и знак подчеркивания.
 */
$table_prefix  = 'wp_';

/**
 * Для разработчиков: Режим отладки WordPress.
 *
 * Измените это значение на true, чтобы включить отображение уведомлений при разработке.
 * Разработчикам плагинов и тем настоятельно рекомендуется использовать WP_DEBUG
 * в своём рабочем окружении.
 * 
 * Информацию о других отладочных константах можно найти в Кодексе.
 *
 * @link https://codex.wordpress.org/Debugging_in_WordPress
 */
define('WP_DEBUG', false);

/* Это всё, дальше не редактируем. Успехов! */

/** Абсолютный путь к директории WordPress. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Инициализирует переменные WordPress и подключает файлы. */
require_once(ABSPATH . 'wp-settings.php');
