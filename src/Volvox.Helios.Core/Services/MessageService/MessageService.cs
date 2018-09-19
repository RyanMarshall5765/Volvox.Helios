﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Volvox.Helios.Core.Bot;
using System.Linq;

namespace Volvox.Helios.Core.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly IBot _bot;

        public MessageService(IBot bot)
        {
            _bot = bot;
        }

        ///<inheritdoc />
        public async Task<IUserMessage> GetMessage(ulong channelId, ulong messageId)
        {
            var channel = GetChannel(channelId);

            var message = await channel.GetMessageAsync(messageId) as IUserMessage;

            return message;
        }

        ///<inheritdoc />
        public Task<IUserMessage> Post(ulong channelId, string text, Embed embed = null, bool isTTS = false, RequestOptions options = null)
        {
            var channel = GetChannel(channelId);

            return channel.SendMessageAsync(text, isTTS, embed, options);
        }

        ///<inheritdoc />
        public Task AddReaction(IUserMessage message, Emoji reaction)
        {
            return message.AddReactionAsync(reaction);
        }

        ///<inheritdoc />
        public async Task<IUserMessage> Modify(ulong channelId, ulong messageId, string text = "", Embed embed = null)
        {
            var message = await GetMessage(channelId, messageId);

            await message.ModifyAsync(m => {
                m.Content = text;
                m.Embed = embed;
            });

            return message;
        }

        ///<inheritdoc />
        public Task Delete(ulong channelId, ulong[] messageIds)
        {
            var channel = GetChannel(channelId);

            return channel.DeleteMessagesAsync(messageIds);
        }

        ///<inheritdoc />
        private IMessageChannel GetChannel(ulong channelId)
        {
            var channel = _bot.Client.GetChannel(channelId) as IMessageChannel;

            if (channel == null)
                throw new InvalidOperationException("Channel doesn't exist");

            return channel;
        }
    }
}
