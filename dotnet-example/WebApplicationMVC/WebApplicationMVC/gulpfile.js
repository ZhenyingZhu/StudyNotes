/// <binding AfterBuild='minify' />
var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');

gulp.task("minify", function () {
    return gulp.src("wwwroot/js/**/*.js")
        .pipe(uglify())
        .pipe(gulp.dest("wwwroot/dist"));
});

gulp.task("default", gulp.series("minify"));