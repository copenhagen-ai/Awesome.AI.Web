/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./src/**/*.{html,js}",
        "./pages/**/*.{cshtml,js}",
        "./areas/**/*.{cshtml,js}"
    ],
    //content: [
    //    './Views/**/*.cshtml',
    //    './wwwroot/**/*.html',
    //    './wwwroot/**/*.js',
    //],
    theme: {
        extend: {
            spacing: {
                lg: '100px',
                md: '50px',
                sm: '15px'
            },
            height: {
                box: '470px',
                full: '100%'
            },
            width: {
                full: '100%',
                eighty: '80%',
                seventy: '70%',
                fifty: '50%',
                '500': '500px'
            }
        }        
    },
    plugins: [],
}

