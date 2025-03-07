const { transformSync } = require('esbuild');

module.exports = {
  process(src, filename) {
    const result = transformSync(src, {
      loader: filename.endsWith('.tsx') ? 'tsx' : 'ts',
      sourcemap: 'inline', // Include source maps for better debugging
      target: 'esnext', // Adjust based on your environment
    });
    return result.code;
  },
};
