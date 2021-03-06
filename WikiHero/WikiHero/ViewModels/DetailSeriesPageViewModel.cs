﻿using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WikiHero.Models;
using WikiHero.Services;
using Xamarin.Essentials;

namespace WikiHero.ViewModels
{
    public class DetailSeriesPageViewModel:BaseViewModel,INavigationAware
    {
        public ObservableCollection<Episode> Episodes { get; set; } 
        public DelegateCommand LoadCommand { get; set; }
        public Serie Serie { get; set; } = new Serie();
        public DelegateCommand ShareCommand { get; set; }
        public DetailSeriesPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IApiComicsVine apiComicsVine) : base(navigationService, dialogService, apiComicsVine)
        {

            ShareCommand = new DelegateCommand(async () =>
            {
                await SharedOpcion();
            });
        }
        async Task LoadEpisode(int idSeries)
        {
            try
            {
                var episodes = await apiComicsVine.FindEpisode(Config.Apikey,idSeries,null);
                Episodes = new ObservableCollection<Episode>(episodes.Results);
            }
            catch (Exception err)
            {

               await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        async Task SharedOpcion()
        {
            await Share.RequestAsync(new ShareTextRequest
            {

                Text = $"{Serie.Name}\nCantidad de episodios:{Serie.CountOfEpisodes}",
                Title = $"{Serie.Publisher.Name}",
                Uri = $"{Serie.SiteDetailUrl}"
            });
        }
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var param = (Serie)parameters[nameof(Serie)];
            Serie = param;
            LoadCommand = new DelegateCommand(async () => await LoadEpisode(Serie.Id));
            LoadCommand.Execute();
        }
    }
}
