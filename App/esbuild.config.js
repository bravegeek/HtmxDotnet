const esbuild = require('esbuild');
const glob = require('glob');

const buildJs = async () => {
    await esbuild.build({
        entryPoints: [
            ...glob.sync('JavaScript/site/**/*.js'), // Match all JS files in the site directory
            'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js' // Include Bootstrap JS
        ],
        bundle: true,
        minify: true,
        outdir: 'wwwroot/dist/js/site', // Output directory for site JS
        sourcemap: true,
        target: ['esnext'], // Set ECMAScript version
    });
}

const buildCss = async () => {
    await esbuild.build({
        entryPoints: [
            'CSS/site.css',
        ],
        bundle: true,
        minify: true,
        outdir:'wwwroot/dist/css',
        sourcemap: true,
        loader: {
            '.css': 'css' // Ensure ESBuild knows it's processing CSS
        },
    });
}


const buildGameTs = async () => {
    await esbuild.build({
        entryPoints: [
            'JavaScript/game/index.ts'
        ],
        bundle: true,
        minify: true,
        sourcemap: true,
        outdir: 'wwwroot/dist/js/game',
        loader: {
            '.ts': 'ts'
        }
    });
}


// Function to watch changes
const watchSite = async () => {
    await esbuild.build({
        entryPoints: [
            ...glob.sync('JavaScript/site/**/*.js'),
            'node_modules/bootstrap/dist/js/bootstrap.bundle.min.js' // Include Bootstrap JS
        ],
        bundle: true,
        outdir: 'wwwroot/dist/js/site',
        watch: true,
        target: ['es6'],
    });

    await esbuild.build({
        entryPoints: [
            'CSS/site.css',
        ],
        bundle: true,
        outdir: 'wwwroot/dist/css',
        watch: true,
    });

};

const watchGame = async () => {
    esbuild.build({
        entryPoints: ['JavaScript/game/index.ts'],
        bundle: true,
        outdir: 'wwwroot/dist/js/game',
        watch: true,
        loader: {
            '.ts': 'ts'
        }
    });
}


// Main function to build based on command line arguments
const run = async () => {
    const args = process.argv.slice(2);
    
    if (args.includes('buildSite')) {
        await buildJs();
        await buildCss();
        return;
    }
    
    if (args.includes('buildGame')) {
        await buildGameTs();
        return;
    } 
    
    if (args.includes('watchSite')) {
        await watchSite();
        return;
    }

    if(args.includes('watchGame')){
        await watchGame();
        return;
    }

    console.log('Invalid command. Use "buildSite", "buildGame", or "watchSite".');
};
