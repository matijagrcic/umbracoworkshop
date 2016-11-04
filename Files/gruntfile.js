module.exports = function (grunt) {
    "use strict";

    //we can use specified compiler, by passing --typeScriptVersion argument, for example grunt typeScriptVersion=1.1
    //Microsoft installs TypeScript tools in separate directories C:\Program Files (x86)\Microsoft SDKs\TypeScript\[VERSION]\tsc
    //so we can install several versions and then for each project use the version we want

    //use TypeScript 1.7 if nothing is passed in node_modules/typescript/
    //var typeScriptVersion = grunt.option('typeScriptVersion') || '1.7';
    //var typeScriptPath = 'C:\\Program Files (x86)\\Microsoft SDKs\\TypeScript\\' + typeScriptVersion + '\\tsc';

    // load all grunt tasks
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        watch: {
            options: {
                forever: true
            },
            ts: {
                options: { interrupt: true, livereload: true },
                files: ['App_Plugins/**/*.ts', 'Scripts/**/*.ts'],
                tasks: ['ts']
            },
            less: {
                files: ['App_Plugins/**/*.less'],
                tasks: ['less']
            }
        },
        ts: {
            options: {
                compile: true,          // perform compilation. [true (default) | false]
                comments: false,        // same as !removeComments. [true | false (default)]
                target: 'es5',          // target javascript language. [es3 | es5 (grunt-ts default) | es6]
                module: 'amd',          // target javascript module style. [amd (default) | commonjs]
                sourceMap: false,       // generate a source map for every output js file. [true (default) | false]
                declaration: false,     // generate a declaration .d.ts file for every output js file. [true | false (default)]
                fast: "watch",          // see https://github.com/TypeStrong/grunt-ts/blob/master/docs/fast.md ["watch" (default) | "always" | "never"]
                //compiler: typeScriptPath //useful for debugging local and build
            },
            core: {
                src: ["Scripts/**/*.ts"],
                out: 'Scripts/core.js'
            },
            plugins: {
                files: [{
                    src: ["App_Plugins/**/*.ts", "!App_Plugins/tweaks/tweaks.ts"],
                    dest: 'App_Plugins/plugins.js'
                },
                 {
                     src: ["App_Plugins/**/tweaks.ts"],
                     dest: 'App_Plugins/Tweaks/js/tweaks.js'
                 }]
            }
        },
        less: {
            plugins: {
                files: [{
                    src: ["App_Plugins/**/*.less", "!App_Plugins/tweaks/style.less"],
                    dest: "App_Plugins/plugins.css"
                },
                {
                    src: ["App_Plugins/tweaks/style.less"],
                    dest: "App_Plugins/tweaks/css/tweaks.css"
                }]
            }
        }
    });

    grunt.loadNpmTasks("grunt-ts");
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.registerTask('default', function () {
        var tasks = ['ts', 'less'];
        grunt.option('force', true);
        grunt.task.run(tasks);
    });
};