var gulp        = require('gulp');
var sass        = require('gulp-sass');
var minifyCSS   = require('gulp-clean-css');
var concat      = require('gulp-concat');
var sourcemaps  = require('gulp-sourcemaps');
var download    = require('gulp-download-stream');
var uglify      = require('gulp-uglify');

gulp.task('css', function(){
  return gulp.src([
  	'src/scss/font.scss',
  	'node_modules/@fortawesome/fontawesome-free/css/all.min.css',
  	'node_modules/jquery-ui-dist/jquery-ui.min.css',
  	'node_modules/animate.css/animate.min.css',
  	'node_modules/pace-js/themes/black/pace-theme-flash.css',
  	'src/scss/styles.scss'
  	])
    .pipe(sass())
    .pipe(concat('app.min.css'))
    .pipe(minifyCSS())
    .pipe(gulp.dest('wwwroot/css/'));
});

gulp.task('css-img', function(){
	return gulp.src([ 'src/scss/images/**' ])
        .pipe(gulp.dest('wwwroot/css/images'));
});

gulp.task('fonts', function() {
  return gulp.src(['node_modules/@fortawesome/fontawesome-free/webfonts/*'])
        .pipe(gulp.dest('wwwroot/webfonts/'));
});

gulp.task('img', function() {
    return gulp.src(['src/img/**'])
        .pipe(gulp.dest('wwwroot/img/'));
});

gulp.task('plugins', function() {
    download([
        'http://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.18.1/styles/default.min.css',
        'http://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.18.1/highlight.min.js'
    ]).pipe(gulp.dest('wwwroot/lib/highlight.js/'));
	
	var pluginFiles = [
        'node_modules/apexcharts/**',
        'node_modules/chart.js/**',
        'node_modules/jvectormap/**',
        'node_modules/bootstrap-colorpicker/**',
        'node_modules/bootstrap-datepicker/**',
        'node_modules/bootstrap-daterangepicker/**',
        'node_modules/bootstrap-select/**',
        'node_modules/bootstrap-slider/**',
        'node_modules/bootstrap-tagsinput/**',
        'node_modules/bootstrap-timepicker/**',
        'node_modules/bootstrap-3-typeahead/**',
        'node_modules/bootstrap-table/**',
        'node_modules/bs-custom-file-input/**',
        'node_modules/jquery.maskedinput/**',
        'node_modules/pwstrength-bootstrap/**',
        'node_modules/summernote/**',
        'node_modules/@fullcalendar/**',
        'node_modules/blueimp-file-upload/**',
        'node_modules/blueimp-tmpl/**',
        'node_modules/blueimp-gallery/**',
        'node_modules/blueimp-canvas-to-blob/**',
        'node_modules/blueimp-load-image/**',
        'node_modules/datatables.net/**',
        'node_modules/datatables.net-bs4/**',
        'node_modules/datatables.net-autofill/**',
        'node_modules/datatables.net-autofill-bs4/**',
        'node_modules/datatables.net-buttons/**',
        'node_modules/datatables.net-buttons-bs4/**',
        'node_modules/datatables.net-colreorder/**',
        'node_modules/datatables.net-colreorder-bs4/**',
        'node_modules/datatables.net-fixedcolumns/**',
        'node_modules/datatables.net-fixedcolumns-bs4/**',
        'node_modules/datatables.net-fixedheader/**',
        'node_modules/datatables.net-fixedheader-bs4/**',
        'node_modules/datatables.net-keytable/**',
        'node_modules/datatables.net-keytable-bs4/**',
        'node_modules/datatables.net-responsive/**',
        'node_modules/datatables.net-responsive-bs4/**',
        'node_modules/datatables.net-rowgroup/**',
        'node_modules/datatables.net-rowgroup-bs4/**',
        'node_modules/datatables.net-rowreorder-bs4/**',
        'node_modules/datatables.net-scroller/**',
        'node_modules/datatables.net-scroller-bs4/**',
        'node_modules/datatables.net-select/**',
        'node_modules/datatables.net-select-bs4/**',
        'node_modules/photoswipe/**',
        'node_modules/kbw-countdown/**',
        'node_modules/moment/**'
	];
    return gulp.src(pluginFiles, { base: './node_modules/' }).pipe(gulp.dest('wwwroot/lib'));
});

gulp.task('js', function(){
    gulp.src('src/js/demo/**')
        .pipe(gulp.dest('wwwroot/js/demo/'));
    return gulp.src([
        'node_modules/pace-js/pace.min.js',
        'node_modules/jquery/dist/jquery.min.js',
        'node_modules/jquery-ui-dist/jquery-ui.min.js',
        'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js',
        'node_modules/jquery-slimscroll/jquery.slimscroll.min.js',
        'node_modules/js-cookie/src/js.cookie.js',
        'src/js/app.js',
    ])
    .pipe(sourcemaps.init())
    .pipe(concat('app.min.js'))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('wwwroot/js'));
});

gulp.task('default', gulp.series(gulp.parallel([ 'css', 'css-img', 'js', 'fonts', 'img'])));