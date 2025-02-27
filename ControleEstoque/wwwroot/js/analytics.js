document.addEventListener("DOMContentLoaded", () => {
const divPeriodoData = document.querySelector(".divPeriodoData")
const select = document.querySelector("#selctOptions")
const switchData = document.querySelector("#switchData")
const selectAcao = document.querySelector('#selctAçao')
const selctProd = document.querySelector('#slectMaterial')
const selectTamanho = document.querySelector('#slectTamanho')
const graphArea = document.querySelector('.graphs')
const legendaArea = document.querySelector('.legenda')
//dps fazer um sprad para os outros btns gerados 
const btnMoreGraphsArea = document.querySelector('.moreGraphs')
const containerAnalytcs = document.querySelector('.containerAnalytics')

    let count = 2;
function factoryDiv()
{
    return document.createElement("div")
}
function factoryBtn()
{
    return document.createElement('button')
}

function factorySelect(class1, id) {
    let placeholder = document.createElement("select")

    placeholder.classList.add('form-select')
    placeholder.classList.add('selctConfig')
    placeholder.classList.add('selectPrincSecu')
    placeholder.classList.add(class1)
    placeholder.setAttribute('id',id)

    return placeholder

}
function factoryOption(value, name) {
    let placeholder = document.createElement('option')
    placeholder.setAttribute('value', value)
    placeholder.textContent=name
    return placeholder
}

switchData.addEventListener('change', () =>
{
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
        graphArea.classList.remove("graphs2", "graphs3");
        graphArea.classList.add("graphs")
        legendaArea.classList.remove("legenda2", "legenda3");

        legendaArea.classList.add('legenda')
    }
    else if (selectTamanho.value == 2) {
        graphArea.classList.remove("graphs", "graphs3");
        graphArea.classList.add('graphs2')

        legendaArea.classList.remove("legenda", "legenda3");
        legendaArea.classList.add('legenda2')
    }
    else {
        graphArea.classList.remove("graphs", "graphs2");
        graphArea.classList.add('graphs3')

        legendaArea.classList.remove("legenda", "legenda2");
        legendaArea.classList.add('legenda3')
    }
})

function createNewArea() 
{

    let divBotGraphs = factoryDiv();
    let divHeader = factoryDiv();
    let divSeletores = factoryDiv();
    let divGraphComp = factoryDiv();
    let divGraphArea = factoryDiv();
    let divGraphs = factoryDiv();
    let divLegenda = factoryDiv();
    let divBtns = factoryDiv();
    let btnMore = factoryBtn();
    let btnLess = factoryBtn();

    //corpo
    divBotGraphs.classList.add("botGraphs", "mt-3");
    divBotGraphs.setAttribute('id', `graficoBot${count}`)

    //header
    let h1 = document.createElement('h1');
    h1.classList.add('ms-5');
    h1.textContent = "Gráficos da empresa";

    let image = document.createElement('i');
    image.classList.add("bi", "bi-plus");
    image.style.color = 'white';

    let imageX = document.createElement('i');
    imageX.classList.add("bi", "bi-x-lg");
    imageX.style.color = 'white';

    btnMore.classList.add("btn","btn-success","text-light","text-center","me-5");
    btnMore.appendChild(image)

    btnLess.classList.add("btn","btn-danger","text-light","text-center","me-5");
    btnLess.appendChild(imageX)

    divHeader.classList.add("headerGraphs","d-flex","justify-content-between","align-items-center")
    divBtns.classList.add('btns')

    divHeader.appendChild(h1);
    divBtns.appendChild(btnLess);
    divBtns.appendChild(btnMore);
    divHeader.appendChild(divBtns);

    //comparação
    divGraphComp.classList.add('graphsComparação')

    //seletores
    divSeletores.classList.add("seletores", "d-flex", "justify-content-evenly", "align-items-center")
    //slects
    //tipos de graficos 
    let selectStyleGprah = factorySelect('slectGraphs', `slectGraphs${count}`);
    let graphOpDefalt = factoryOption(null, "Escolha um tipo de grafico")
    graphOpDefalt.setAttribute('selected','true')
    let graphOp1 = factoryOption(1, "Pizza")
    let graphOp2 = factoryOption(2,"Barras")
    let graphOp3 = factoryOption(3, "Linha")
    selectStyleGprah.appendChild(graphOpDefalt)
    selectStyleGprah.appendChild(graphOp1)
    selectStyleGprah.appendChild(graphOp2)
    selectStyleGprah.appendChild(graphOp3)
    //açoes 
    let selectAcao = factorySelect('selctAçao', `selctAçao${count}`);
    let acaoOpDefalt = factoryOption(null, "Escolha a informação do grafico")
    acaoOpDefalt.setAttribute('selected', 'true')
    let acaoOp1 = factoryOption(1, "Verificar Material")
    let acaoOp2 = factoryOption(2, "Número de Pedidos")
    let acaoOp3 = factoryOption(3, "Pedidos Realizados")
    let acaoOp4 = factoryOption(4, "Pedidos Concluidos")
    let acaoOp5 = factoryOption(5, "Lucro")
    let acaoOp6 = factoryOption(6, "Gastos")
    let acaoOp7 = factoryOption(7, "Materiais Gastos total")
    selectAcao.appendChild(acaoOpDefalt)
    selectAcao.appendChild(acaoOp1)
    selectAcao.appendChild(acaoOp2)
    selectAcao.appendChild(acaoOp3)
    selectAcao.appendChild(acaoOp4)
    selectAcao.appendChild(acaoOp5)
    selectAcao.appendChild(acaoOp6)
    selectAcao.appendChild(acaoOp7)

    //data
    let selectData = factorySelect('slectGraphs', `selectData${count}`);
    let dataOpDefalt = factoryOption(null, "Escolha uma das opções abaixo")
    graphOpDefalt.setAttribute('selected', 'true')
    let dataOp1 = factoryOption(1, "Semana")
    let dataOp2 = factoryOption(2, "Mês")
    let dataOp3 = factoryOption(3, "Ano")
    selectData.appendChild(dataOpDefalt)
    selectData.appendChild(dataOp1)
    selectData.appendChild(dataOp2)
    selectData.appendChild(dataOp3)
    //material
    let selectMaterial = factorySelect('slectGraphs', `selectMaterial${count}`);
    selectMaterial.setAttribute('disabled','true')
    let materialOpDefalt = factoryOption(null, "Escolha um material")
    materialOpDefalt.setAttribute('selected', 'true')
    let materialOp1 = factoryOption(1, "Tabuas")
    let materialOp2 = factoryOption(2, "Parafusos")
    let materialOp3 = factoryOption(3, "outro prod")
    selectMaterial.appendChild(materialOpDefalt)
    selectMaterial.appendChild(materialOp1)
    selectMaterial.appendChild(materialOp2)
    selectMaterial.appendChild(materialOp3)


    selectAcao.addEventListener('change', (e) => {
        let id = e.target.closest('.botGraphs').id
        let numero = id.match(/\d+$/)[0];
        let selectedMateril = document.getElementById(`selectMaterial${numero}`)
        if (selectAcao.value == 1) {
            selectedMateril.removeAttribute('disabled')
        } else {
            selectedMateril.setAttribute('disabled', 'true')
        }
    })

    //grafrico

    divGraphArea.classList.add("graphsArea", "d-flex", "justify-content-center", "align-items-center")

    if (selectTamanho.value == 1) {
        divGraphs.classList.add("graphs")

        divLegenda.classList.add('legenda')
    }
    else if (selectTamanho.value == 2) {

        divGraphs.classList.add('graphs2')


        divLegenda.classList.add('legenda2')
    }
    else {
        divGraphs.classList.add('graphs3')

        divLegenda.classList.add('legenda3')
    }
    selectTamanho.addEventListener('change', () => {
        if (selectTamanho.value == 1) {
            divGraphs.classList.remove("graphs2", "graphs3");
            divGraphs.classList.add("graphs")

            divLegenda.classList.remove("legenda2", "legenda3");
            divLegenda.classList.add('legenda')
        }
        else if (selectTamanho.value == 2) {
            divGraphs.classList.remove("graphs", "graphs3");
            divGraphs.classList.add('graphs2')

            divLegenda.classList.remove("legenda", "legenda3");
            divLegenda.classList.add('legenda2')
        }
        else {
            divGraphs.classList.remove("graphs", "graphs2");
            divGraphs.classList.add('graphs3')

            divLegenda.classList.remove("legenda", "legenda2");
            divLegenda.classList.add('legenda3')
        }
    })

    divGraphArea.appendChild(divGraphs)
    divGraphArea.appendChild(divLegenda)

    divSeletores.appendChild(selectStyleGprah)
    divSeletores.appendChild(selectAcao)
    divSeletores.appendChild(selectData)
    divSeletores.appendChild(selectMaterial)

    divGraphComp.appendChild(divSeletores)
    divGraphComp.appendChild(divGraphArea)
    divBotGraphs.appendChild(divHeader);
    divBotGraphs.appendChild(divGraphComp);
    containerAnalytcs.appendChild(divBotGraphs);
    count++
    btnLess.addEventListener('click', (e) => {
        let idElement = e.target.closest('.botGraphs').id
        let elementHtml = document.querySelector(`#${idElement}`)
        elementHtml.remove()
    })
    btnMore.addEventListener('click', createNewArea)
    }
    btnMoreGraphsArea.addEventListener('click', createNewArea)

});
