﻿using Svt.Caspar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TAS.Common;
using TAS.Remoting.Server;
using TAS.Server.Interfaces;

namespace TAS.Server
{

    public class CasparRecorder: DtoBase, IRecorder
    {
        internal CasparServer ownerServer;
        private TVideoFormat _tcFormat = TVideoFormat.PAL;
        private Recorder _recorder;
        internal void SetRecorder(Recorder value)
        {
            var oldRecorder = _recorder;
            if (_recorder != value)
            {
                if (oldRecorder != null)
                {
                    oldRecorder.Tc -= _recorder_Tc;
                    oldRecorder.DeckConnected -= _recorder_DeckConnected;
                    oldRecorder.DeckControl -= _recorder_DeckControl;
                    oldRecorder.DeckState -= _recorder_DeckState;
                }
                _recorder = value;
                if (value != null)
                {
                    value.Tc += _recorder_Tc;
                    value.DeckConnected += _recorder_DeckConnected;
                    value.DeckControl += _recorder_DeckControl;
                    value.DeckState += _recorder_DeckState;
                    IsConnected = value.IsConnected;
                    DeckState = TDeckState.Unknown;
                    DeckControl = TDeckControl.None;
                }
            }
        }

        private void _recorder_DeckState(object sender, DeckStateEventArgs e)
        {
            DeckState = (TDeckState)e.State;
        }

        private void _recorder_DeckControl(object sender, DeckControlEventArgs e)
        {
            DeckControl = (TDeckControl)e.ControlEvent;
        }

        private void _recorder_DeckConnected(object sender, DeckConnectedEventArgs e)
        {
            IsConnected = e.IsConnected;
        }

        private void _recorder_Tc(object sender, TcEventArgs e)
        {
            if (e.Tc.IsValidSMPTETimecode(_tcFormat))
                CurrentTc = e.Tc.SMPTETimecodeToTimeSpan(_tcFormat);
        }


        #region Deserialized properties
        public int RecorderNumber { get; set; }
        public int Id { get; set; }
        public string RecorderName { get; set; }
        #endregion Deserialized properties

        private TimeSpan _currentTc;
        [XmlIgnore]
        public TimeSpan CurrentTc { get { return _currentTc; }  private set { SetField(ref _currentTc, value, nameof(CurrentTc)); } }

        private TDeckState _deckState;
        [XmlIgnore]
        public TDeckState DeckState { get { return _deckState; } private set { SetField(ref _deckState, value, nameof(DeckState)); } }

        private TDeckControl _deckControl;
        [XmlIgnore]
        public TDeckControl DeckControl { get { return _deckControl; }  private set { SetField(ref _deckControl, value, nameof(DeckControl)); } }

        private bool _isConnected;
        [XmlIgnore]
        public bool IsConnected { get { return _isConnected; } private set { SetField(ref _isConnected, value, nameof(IsConnected)); } }

        public IEnumerable<IPlayoutServerChannel> Channels { get { return ownerServer.Channels; } }

        public void Capture(IPlayoutServerChannel channel, TimeSpan tcIn, TimeSpan tcOut, string fileName)
        {
            _tcFormat = channel.VideoFormat;
            _recorder?.Capture(channel.Id, tcIn.ToSMPTETimecodeString(channel.VideoFormat), tcOut.ToSMPTETimecodeString(channel.VideoFormat), fileName);
        }
        public void Abort()
        {
            _recorder?.Abort();
        }

        public void Play()
        {
            _recorder?.Play();
        }
        public void Stop()
        {
            _recorder?.Stop();
        }

        public void FastForward()
        {
            _recorder.FastForward();
        }

        public void Rewind()
        {
            _recorder.Rewind();
        }

        public void GoToTimecode(TimeSpan tc, TVideoFormat format)
        {
            _recorder?.GotoTimecode(tc.ToSMPTETimecodeString(format));
        }

        private IServerMedia _recordingMedia;
        [XmlIgnore]
        public IServerMedia RecordingMedia { get { return _recordingMedia; } private set { SetField(ref _recordingMedia, value, nameof(RecordingMedia)); } }

        public IServerDirectory RecordingDirectory { get { return ownerServer.MediaDirectory; } }
                
    }
}