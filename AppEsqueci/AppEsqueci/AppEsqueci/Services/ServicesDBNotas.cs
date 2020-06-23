﻿using System;
using System.Collections.Generic;
using System.Text;
using AppEsqueci.Models;
using SQLite;

namespace AppEsqueci.Services
{
    public class ServicesDBNotas
    {
        SQLiteConnection conn;
        public string StatusMessage { get; set; }
        public ServicesDBNotas(string dbPath)
        {
            if (dbPath == "") dbPath = App.DbPath;
            conn = new SQLiteConnection(dbPath); //define o banco 
            conn.CreateTable<ModelNotas>(); //cria a tabela notas
        }

        public void Inserir(ModelNotas nota)
        {
            try
            {
                if (string.IsNullOrEmpty(nota.Titulo))
                    throw new Exception("Titulo da nota não informado");
                if (string.IsNullOrEmpty(nota.Dados))
                    throw new Exception("Dados da nota não informados");
                int result = conn.Insert(nota);
                if (result != 0)
                {
                    this.StatusMessage =
                        string.Format("{0} registro(s) adicionado(s): [Nota: {1}]", 
                        result, nota.Titulo);
                }
                else
                {
                    string.Format("0 registro(s) adicionado(s)");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }
        public List<ModelNotas> Listar()
        {
            List<ModelNotas> lista = new List<ModelNotas>();
            try
            {
                lista = conn.Table<ModelNotas>().ToList();
                this.StatusMessage = "Listagem das Notas";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }
        public void Alterar(ModelNotas nota)
        {
            try
            {
                if (string.IsNullOrEmpty(nota.Titulo))
                    throw new Exception("Titulo da nota não informado");
                if (string.IsNullOrEmpty(nota.Dados))
                    throw new Exception("Dados da nota não informados");
                if (nota.Id <= 0)
                    throw new Exception("Id da nota não informado");
                int result = conn.Update(nota);
                StatusMessage = string.Format("{0} Registros alterados.", result);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }

        public void Excluir(int id)
        {
            try
            {
                int result = conn.Table<ModelNotas>().Delete(r => r.Id == id);
                StatusMessage = string.Format("{0} Registros deletados.",result);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
        }
        public List<ModelNotas> Localizar(string titulo)
        {
            List<ModelNotas> lista = new List<ModelNotas>();
            try
            {
                var resp = from p in conn.Table<ModelNotas>()
                           where p.Titulo.ToLower().Contains(titulo.ToLower())
                           select p;
                lista = resp.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
            return lista;
        }
        public List<ModelNotas> Localizar(string titulo, Boolean favorito)
        {
            List<ModelNotas> lista = new List<ModelNotas>();
            try
            {
                var resp = from p in conn.Table<ModelNotas>()
                           where p.Titulo.ToLower().Contains(titulo.ToLower()) && 
                           p.Favorito == favorito
                           select p;
                lista = resp.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
            return lista;
        }
        public List<ModelNotas> ListarFavoritos()
        {
            List<ModelNotas> lista = new List<ModelNotas>();
            try
            {
                var resp = from p in conn.Table<ModelNotas>()
                           where p.Favorito == true
                           select p;
                lista = resp.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
            return lista;
        }
        public ModelNotas GetNota(int id)
        {
            ModelNotas m = new ModelNotas();
            try
            {
                m = conn.Table<ModelNotas>().First(n => n.Id == id);
                StatusMessage = "Encontrou a nota";
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Erro: {0}", ex.Message));
            }
            return m;
        }
    }
}
