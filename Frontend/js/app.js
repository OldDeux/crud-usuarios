const API_URL = "http://localhost:5065/api/usuarios";

async function carregarUsuarios() {
    try {
        const resposta = await fetch(API_URL);

        if (!resposta.ok) {
            throw new Error("Erro ao buscar usuários.");
        }

        const usuarios = await resposta.json();

        mostrarUsuarios(usuarios);

    } catch (erro) {
        console.error(erro);
        alert("Não foi possível carregar os usuários.");
    }
}

async function cadastrarUsuario(event) {
    event.preventDefault();

    const usuario = {
        nome: document.getElementById("nome").value,
        email: document.getElementById("email").value,
        cpf: document.getElementById("cpf").value,
        telefone: document.getElementById("telefone").value,
        dataNascimento: document.getElementById("dataNascimento").value
    };

    try {
        const resposta = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(usuario)
        });

        if (!resposta.ok) {
            const erro = await resposta.text();
            throw new Error(erro);
        }

        alert("Usuário cadastrado com sucesso!");

        document.getElementById("formUsuario").reset();

        await carregarUsuarios();

    } catch (erro) {
        console.error(erro);
        alert(erro.message);
    }
}

function mostrarUsuarios(usuarios) {
    const tabela = document.getElementById("tabelaUsuarios");

    tabela.innerHTML = "";

    usuarios.forEach(usuario => {

        const linha = document.createElement("tr");

        linha.innerHTML = `
            <td>${usuario.id}</td>
            <td>${usuario.nome}</td>
            <td>${usuario.email}</td>
            <td>${usuario.cpf}</td>
            <td>${usuario.telefone ?? ""}</td>
            <td>${formatarData(usuario.dataNascimento)}</td>
        `;

        tabela.appendChild(linha);
    });
}

function formatarData(data) {
    if (!data) {
        return "";
    }

    return new Date(data).toLocaleDateString("pt-BR");
}

document
    .getElementById("formUsuario")
    .addEventListener("submit", cadastrarUsuario);

carregarUsuarios();