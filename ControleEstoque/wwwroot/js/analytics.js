const divPeriodoData = document.querySelector(".divPeriodoData")
const select = document.querySelector("#selctOptions")
const switchData = document.querySelector("#switchData")
const selectAcao = document.querySelector('#selctAçao')
const selctProd = document.querySelector('#slectProd')
const selectTamanho = document.querySelector('#slectTamanho')
const graphArea = document.querySelector('.graphs')
const legendaArea = document.querySelector('.legenda')


switchData.addEventListener('change', () =>
{
    console.log(switchData.checked)
    if (!switchData.checked) {
        select.setAttribute('disabled', 'true')
        divPeriodoData.classList.remove("ivisivel")
       

    } else
    {
        select.removeAttribute('disabled')
        divPeriodoData.classList.add("ivisivel")
    }
})

selectAcao.addEventListener('change', () =>
{
    if (selectAcao.value == 1) {
        selctProd.removeAttribute('disabled')
    } else
    {
        selctProd.setAttribute('disabled','true')
    }
})

selectTamanho.addEventListener('change', () =>
{
    if (selectTamanho.value == 1) {
        graphArea.classList.remove("graphs2")
        graphArea.classList.remove('graphs3')
        graphArea.classList.add("graphs")

        legendaArea.classList.remove('legenda2')
        legendaArea.classList.remove('legenda3')
        legendaArea.classList.add('legenda')
    }
    else if (selectTamanho.value == 2) {
        graphArea.classList.remove("graphs")
        graphArea.classList.remove('graphs3')
        graphArea.classList.add('graphs2')

        legendaArea.classList.remove("legenda")
        legendaArea.classList.remove('legenda3')
        legendaArea.classList.add('legenda2')
    }
    else {
        graphArea.classList.remove('graphs')
        graphArea.classList.remove('graphs2')
        graphArea.classList.add('graphs3')

        legendaArea.classList.remove('legenda')
        legendaArea.classList.remove('legenda2')
        legendaArea.classList.add('legenda3')
    }
})