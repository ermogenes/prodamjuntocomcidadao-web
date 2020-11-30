const abrirDetalhes = (e) => {
    e.preventDefault();
    const detalhes = document.getElementById("novo-detalhes");
    detalhes.classList.remove("invisivel");
    const self = e.currentTarget;
    self.outerHTML = "";
};

const abandonar = async (e) => {
    e.preventDefault();
    const self = e.currentTarget;
    self.innerHTML = "carregando...";

    const feed = document.getElementById("feed");
    feed.classList.remove("invisivel");

    preencheFeed();

    const novo = document.getElementById("nova-mensagem");
    novo.outerHTML = "";

    self.outerHTML = "";
};

const curtir = async (e) => {
    e.preventDefault();
    const link = e.currentTarget;
    const url = `api/${link.dataset.controller}/${link.dataset.id}/curtir`;
    if (!localStorage.getItem(`prodamjuntocomcidadao.curtida.${link.dataset.id}`))
    {
        const response = await fetch(url, {
            method: "PATCH",
        });

        if (response.ok) {
            const result = await response.json();
            link.innerHTML = `👍 ${result.curtidas}`;
            localStorage.setItem(`prodamjuntocomcidadao.curtida.${link.dataset.id}`, true);
        } else {
            alert("Erro!");
        }
    }
};

const enviaMensagem = async (e) => {
    e.preventDefault();

    const botao = document.getElementById("novo-abandonar");

    const mensagem = {
        "TipoId": document.getElementById("novo-tipo")?.value,
        "LocalId": document.getElementById("novo-local")?.value,
        "TemaId": document.getElementById("novo-tema")?.value,
        "Texto": document.getElementById("novo-texto")?.value,
    };

    if (!mensagem.Texto) {
        alert("Por favor, digite uma mensagem.");
        return;
    }

    if (mensagem.Texto.length > 400) {
        alert("Por favor, digite uma mensagem com até 400 caracteres.");
        return;
    }

    botao.innerHTML = "carregando...";

    const response = await fetch("api/Mensagens", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(mensagem),
    });
    if (response.ok) {
        botao.dispatchEvent(new Event("click"));
    } else {
        alert("Erro!");
    }
};

const preencheListas = async () => {
    const responseTemas = await fetch("api/Temas");
    const resultTemas = await responseTemas.json();
    const listaTemas = document.getElementById("temas");
    // resultTemas.forEach(tema => {
    //     listaTemas.insertAdjacentHTML("beforeend", `<li>${tema.nome} <a class="curtir" data-controller="Temas" data-id="${tema.id}">👍 ${tema.curtidas}</a></li>`);
    // });
    const selectTemas = document.getElementById("novo-tema");
    resultTemas.forEach(tema => {
        selectTemas.insertAdjacentHTML("beforeend", `<option value="${tema.id}">${tema.nome}</option>`);
    });

    const responseTipos = await fetch("api/Tipos");
    const resultTipos = await responseTipos.json();
    // const listaTipos = document.getElementById("tipos");
    // resultTipos.forEach(tipo => {
    //     listaTipos.insertAdjacentHTML("beforeend", `<li>${tipo.nome} <a class="curtir" data-controller="Tipos" data-id="${tipo.id}">👍 ${tipo.curtidas}</a></li>`);
    // });
    const selectTipos = document.getElementById("novo-tipo");
    resultTipos.forEach(tipo => {
        selectTipos.insertAdjacentHTML("beforeend", `<option value="${tipo.id}">${tipo.nome}</option>`);
    });
    
    const responseLocais = await fetch("api/Locais");
    const resultLocais = await responseLocais.json();
    // const listaLocais = document.getElementById("locais");
    // resultLocais.forEach(local => {
    //     listaLocais.insertAdjacentHTML("beforeend", `<li>${local.nome} <a class="curtir" data-controller="Locais" data-id="${local.id}">👍 ${local.curtidas}</a></li>`);
    // });
    const selectLocais = document.getElementById("novo-local");
    resultLocais.forEach(local => {
        selectLocais.insertAdjacentHTML("beforeend", `<option value="${local.id}">${local.nome}</option>`);
    });
};

const preencheFeed = async () => {
    const responseMensagens = await fetch(`api/Mensagens?skip=${skipMensagens}`);
    if (responseMensagens.ok) skipMensagens += 10;
    const resultMensagens = await responseMensagens.json();
    const listaMensagens = document.getElementById("mensagens");
    resultMensagens.forEach(msg => {
        var datahora = new Date(msg.data);
        // Fixo timezone West US #TODO refatorar para i18n
        const data = moment(datahora).add(-3, 'hours').fromNow();
        const emoji = msg.sentimentScore < 0.33 ? "🤬" : msg.sentimentScore > 0.66 ? "🥰" : "🤔";
        const score = (msg.sentimentScore * 100).toFixed(2);
        listaMensagens.insertAdjacentHTML("beforeend", 
`<div class="mensagem-container">
    <div class="mensagem-tipo">${msg.tipo?.nome || ""}</div>
    <div class="mensagem-texto">${msg.texto}</div>
    <a class="curtir" data-controller="Mensagens" data-id="${msg.id}">👍 ${msg.curtidas}</a>
    <div>
    <div class="mensagem-local">${msg.local?.nome || ""}</div>
    <div class="mensagem-tema">${msg.tema?.nome || ""}</div>
    <div class="mensagem-data">${data || ""}</div>
    <div class="mensagem-score"><span alt="${emoji || ""} (${score}%)">${emoji || ""}</span></div>
    </div>
</div>`);
    });
    document.querySelectorAll("a.curtir").forEach(btn => btn.addEventListener("click", curtir));
};

const configuraEventos = () => {
    document.getElementById("novo-confirmar").addEventListener("click", enviaMensagem);
    document.getElementById("novo-abandonar").addEventListener("click", abandonar);
    document.getElementById("novo-detalhes-abrir").addEventListener("click", abrirDetalhes);

    window.addEventListener('scroll', function() {
        if (document.documentElement.scrollTop + document.documentElement.clientHeight >= document.documentElement.scrollHeight) {
            preencheFeed();
        }
    });
};

const iniciar = async () => {
    preencheListas()
        .then(() => configuraEventos());
};

var skipMensagens = 0;

document.addEventListener("DOMContentLoaded", iniciar);