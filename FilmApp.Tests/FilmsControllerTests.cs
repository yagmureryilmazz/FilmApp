using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApp.API.Controllers;
using FilmApp.DataAccess;
using FilmApp.Entities;
using FilmApp.Business.Services;
using FilmApp.Entities.Enums;
using System.Collections.Generic;

namespace FilmApp.Tests
{
    public class FilmsControllerTests
    {
        [Fact]
        public void GetFilms_ShouldReturnFilmList()
        {
            var options = new DbContextOptionsBuilder<FilmDbContext>()
                .UseInMemoryDatabase("TestDb_GetFilms")
                .Options;

            using (var context = new FilmDbContext(options))
            {
                context.Films.Add(new Film("Inception", "Nolan", 2010, 148, Genre.SciFi));
                context.SaveChanges();

                var filmService = new FilmService(context);
                var controller = new FilmsController(filmService);

                var result = controller.GetFilms() as OkObjectResult;

                Assert.NotNull(result);
                var films = Assert.IsType<List<Film>>(result.Value);
                Assert.Single(films);
                Assert.Equal("Inception", films[0].Title);
            }
        }

        [Fact]
        public void AddFilm_ShouldAddFilmSuccessfully()
        {
            var options = new DbContextOptionsBuilder<FilmDbContext>()
                .UseInMemoryDatabase("TestDb_AddFilm")
                .Options;

            using (var context = new FilmDbContext(options))
            {
                var filmService = new FilmService(context);
                var controller = new FilmsController(filmService);

                var newFilm = new Film("The Matrix", "Wachowski", 1999, 136, Genre.SciFi);

                var result = controller.AddFilm(newFilm) as OkObjectResult;

                Assert.NotNull(result);
                var addedFilm = Assert.IsType<Film>(result.Value);
                Assert.Equal("The Matrix", addedFilm.Title);
                Assert.Equal("Wachowski", addedFilm.Director);
            }
        }

        [Fact]
        public void UpdateFilm_ShouldUpdateSuccessfully()
        {
            var options = new DbContextOptionsBuilder<FilmDbContext>()
                .UseInMemoryDatabase("TestDb_UpdateFilm")
                .Options;

            using (var context = new FilmDbContext(options))
            {
                var film = new Film("Old Title", "Old Director", 2000, 100, Genre.Drama);
                context.Films.Add(film);
                context.SaveChanges();

                var filmService = new FilmService(context);
                var controller = new FilmsController(filmService);

                var updatedFilm = new Film("New Title", "New Director", 2022, 120, Genre.Action);

                var result = controller.UpdateFilm(film.Id, updatedFilm) as OkResult;

                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);

                var modified = context.Films.Find(film.Id);
                Assert.Equal("New Title", modified.Title);
                Assert.Equal("New Director", modified.Director);
                Assert.Equal(2022, modified.ReleaseYear);
                Assert.Equal(Genre.Action, modified.Genre);
            }
        }
    }
}

