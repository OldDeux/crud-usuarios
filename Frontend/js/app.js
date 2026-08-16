// URL base da API REST (Back-end ASP.NET Core)
const API_URL = "http://localhost:5065/api/usuarios";

// Busca todos os usuários cadastrados e atualiza a tabela na tela
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

// Trata o envio do formulário: cadastra um novo usuário (POST) ou,
// se o formulário estiver em modo de edição (dataset.id preenchido), atualiza (PUT)
async function cadastrarUsuario(event) {
    event.preventDefault();

    const formulario = document.getElementById("formUsuario");

    const id = formulario.dataset.id;

    const usuario = {
        nome: document.getElementById("nome").value,
        email: document.getElementById("email").value,
        cpf: document.getElementById("cpf").value,
        telefone: document.getElementById("telefone").value,
        dataNascimento: document.getElementById("dataNascimento").value
    };

    try {
        let resposta;

        if (id) {
            resposta = await fetch(`${API_URL}/${id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(usuario)
            });
        } else {
            resposta = await fetch(API_URL, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(usuario)
            });
        }

        if (!resposta.ok) {
            const erro = await resposta.text();
            throw new Error(erro);
        }

        alert(
            id
                ? "Usuário atualizado com sucesso!"
                : "Usuário cadastrado com sucesso!"
        );

        formulario.reset();

        delete formulario.dataset.id;

        document.querySelector(".formulario h2").textContent =
            "Novo usuário";

        document.querySelector("#formUsuario button").textContent =
            "Cadastrar usuário";

        await carregarUsuarios();

    } catch (erro) {
        console.error(erro);
        alert(erro.message);
    }
}

// Renderiza a lista de usuários na tabela HTML
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
            <td>
                <button onclick="editarUsuario(${usuario.id})">
                    Editar
                </button>

                <button onclick="excluirUsuario(${usuario.id})">
                     Excluir
                </button>
            </td>
`;

        tabela.appendChild(linha);
    });
}

// Busca os dados do usuário pelo ID e preenche o formulário em modo de edição
async function editarUsuario(id) {
    try {
        const resposta = await fetch(`${API_URL}/${id}`);

        if (!resposta.ok) {
            throw new Error("Usuário não encontrado.");
        }

        const usuario = await resposta.json();

        document.getElementById("nome").value = usuario.nome;
        document.getElementById("email").value = usuario.email;
        document.getElementById("cpf").value = usuario.cpf;
        document.getElementById("telefone").value = usuario.telefone ?? "";

        document.getElementById("dataNascimento").value =
            usuario.dataNascimento.substring(0, 10);

        document.getElementById("formUsuario").dataset.id = usuario.id;

        document.querySelector(".formulario h2").textContent =
            "Editar usuário";

        document.querySelector("#formUsuario button").textContent =
            "Atualizar usuário";

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });

    } catch (erro) {
        console.error(erro);
        alert(erro.message);
    }
}

// Exclui um usuário após confirmação do usuário no navegador
async function excluirUsuario(id) {
    const confirmar = confirm(
        "Tem certeza que deseja excluir este usuário?"
    );

    if (!confirmar) {
        return;
    }

    try {
        const resposta = await fetch(`${API_URL}/${id}`, {
            method: "DELETE"
        });

        if (!resposta.ok) {
            const erro = await resposta.text();
            throw new Error(erro);
        }

        alert("Usuário excluído com sucesso!");

        await carregarUsuarios();

    } catch (erro) {
        console.error(erro);
        alert(erro.message);
    }
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