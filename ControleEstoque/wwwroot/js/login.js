const inputLogin = document.querySelector("#inputLogin");
const inputSenha = document.querySelector("#inputSenha")

document.addEventListener("DOMContentLoaded", () =>
{
    if (window.innerWidth > 768)
    {
     
        inputLogin.setAttribute("placeholder", "Login:")
        inputSenha.setAttribute("placeholder","Senha:")
    }

})

window.addEventListener("resize", () =>
{
    if (window.innerWidth <= 768)
    {
        inputLogin.removeAttribute("placeholder")
        inputSenha.removeAttribute("placeholder")

    }
    if (window.innerWidth > 768)
    {

        inputLogin.setAttribute("placeholder", "Login:")
        inputSenha.setAttribute("placeholder", "Senha:")
    }
})