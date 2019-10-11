const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        app: "./client/index.tsx"
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/bundles"),
        filename: "[name].js",
        publicPath: "/"
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js"]
    },
    module: {
        rules: [
            {
                test: /\.ts(x?)$/,
                use: "ts-loader"
            },
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            },
            {
                test: /\.(sa|sc|c)ss$/,
                use: [
                    {
                        loader: MiniCssExtractPlugin.loader,
                        options: {
                            hmr: process.env.NODE_ENV === 'development',
                        },
                    },
                    'css-loader',
                    {
                        loader: `postcss-loader`,
                        options: {
                            options: {},
                        }
                    },
                    'sass-loader',
                ],
            },
        ]
    },
    plugins: [
        //new CleanWebpackPlugin(),
        //new HtmlWebpackPlugin({
        //    template: "./src/index.html"
        //}),
        new MiniCssExtractPlugin({
            filename: "css/[name].css"
        }),
    ]
};