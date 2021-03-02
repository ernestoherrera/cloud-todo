module.exports = {
    devServer: {
        proxy: {
            "^/api": {
                
                changeOrigin: true,
                target: 'https://todofunc.azurewebsites.net'
            }
        }
    },
    publicPath: process.env.NODE_ENV === 'production'
        ? 'https://todofunc.azurewebsites.net/api/'
        : ''
}