const iniciar = async () => {
    const responseTemas = await fetch("api/Temas");
    const resultTemas = await responseTemas.json();
    const listaTemas = document.getElementById("temas");
    resultTemas.forEach(tema => {
        listaTemas.insertAdjacentHTML("beforeend", `<li>${tema.nome} 👍 ${tema.curtidas}</li>`);
    });

    const responseTipos = await fetch("api/Tipos");
    const resultTipos = await responseTipos.json();
    const listaTipos = document.getElementById("tipos");
    resultTipos.forEach(tipo => {
        listaTipos.insertAdjacentHTML("beforeend", `<li>${tipo.nome} 👍 ${tipo.curtidas}</li>`);
    });
    
    const responseLocais = await fetch("api/Locais");
    const resultLocais = await responseLocais.json();
    const listaLocais = document.getElementById("locais");
    resultLocais.forEach(local => {
        listaLocais.insertAdjacentHTML("beforeend", `<li>${local.nome} 👍 ${local.curtidas}</li>`);
    });
    
    const responseMensagens = await fetch("api/Mensagens");
    const resultMensagens = await responseMensagens.json();
    const listaMensagens = document.getElementById("mensagens");
    resultMensagens.forEach(mensagem => {
        listaMensagens.insertAdjacentHTML("beforeend", `<li>${mensagem.nome} 👍 ${mensagem.curtidas}</li>`);
    });    
};

document.addEventListener("DOMContentLoaded", iniciar);