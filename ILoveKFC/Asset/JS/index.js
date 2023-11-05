// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };
// w3chool
// When the user scrolls the page, execute myFunction
// Get the navbar
var navbar = document.getElementById("navbar");
// Get the offset position of the navbar
var sticky = navbar.offsetTop;
console.log(sticky, navbar);

function scrollFunction() {

    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("myBtn").style.display = "block";
    } else {
        document.getElementById("myBtn").style.display = "none";
    }
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
    console.log(window.pageYOffset);

}
// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}


// Pham Minh Toan code
const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

// login 
const btnLogin = $('.header_account_login');
const btnRegister = $('.header_account_register')
const formLogin = $('.fancy-box');
const popup = $('.popup');
const btnCloseLoginForm = $('.popup__close')

// product 
const expands = $$('.product__content-iconDown');
const btnItems = $$('.btn_item');
//elements from controller

const App = {

    //login func
    PopupLogin() {
        formLogin.classList.toggle('open');
        popup.classList.toggle('visible');
        stateLogin.classList.remove('visible')
    },
    // product
    expandDescription(e) {
        // lay parent
        const groupArrow = e.target.parentElement;
        const arrowDown = groupArrow.querySelector('.iconDown');
        //const arrowUp = groupArrow.querySelector('.iconUp');
        const describe = groupArrow.parentElement.querySelector('.product__content-describe');
        const ul = describe.querySelector('.list_describe');
        if (arrowDown.classList.contains('visible')) {
            //remove height inline
            describe.setAttribute('style', '');
        }
        else {
            //set height inline
            var count;
            if (ul.children.length > 2) {
                count = 32 * ul.children.length;
            } else {
                count = 64;
            }
            describe.style.height = `${count}px`;
        }
        arrowDown.classList.toggle('visible');
        //arrowUp.classList.toggle('visible');
        console.log(arrowDown.classList.contains('visible'), ul.children.length);
    },


    handleLogin() {

        if (formLogin != null) {
            btnLogin.addEventListener("click", App.PopupLogin);
            btnCloseLoginForm.addEventListener("click", App.PopupLogin)
            formLogin.addEventListener("click", App.PopupLogin)
            popup.addEventListener("click", (e) => {
                e.stopPropagation();
            })

            // khi click nút login và response state là thất bại 
            const stateLogin = $('#stateLogin')
            const response = stateLogin.dataset.state
            // controller Main DangNhap [post] return false
            if (response == "-1") {
                // reload page
                console.log('state of login :' + response)
                App.PopupLogin();
                stateLogin.innerHTML = 'Your username or password is incorrect !'
                setTimeout(() => {
                    stateLogin.classList.add('visible')
                }, 500)
                setTimeout(() => {
                    stateLogin.classList.remove('visible')
                }, 3000)

            }
            else if (response == "0") {
                console.log('state of login :' + response)
                App.PopupLogin();
                setTimeout(() => {
                    stateLogin.classList.add('visible')
                }, 500)
                setTimeout(() => {
                    stateLogin.classList.remove('visible')
                }, 3000)
            }
            else {
                console.log('state of login :' + response)
            }
        }
    },

    handleEvent() {

        this.handleLogin();
        for (var i = 0; i < expands.length; i++) {
            expands[i].addEventListener("click", (e) => App.expandDescription(e));
        }

        // click expand
        expands.forEach(item => {
            item.addEventListener("click", (e) => App.handleOrderProduct(e));
        })
    },

    start() {
        // sau khi login thì không expand dc cái describe
        this.handleEvent();
    }
}

App.start();