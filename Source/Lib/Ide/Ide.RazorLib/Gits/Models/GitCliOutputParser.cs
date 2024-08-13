public class GitCliOutputParser
	public void DispatchSetStatusAction()
		var localRepo = _repo;
		if (localRepo is null)
			return;
	
		_dispatcher.Dispatch(new GitState.SetStatusAction(
			localRepo,
			UntrackedGitFileList.ToImmutableList(),
			StagedGitFileList.ToImmutableList(),
			UnstagedGitFileList.ToImmutableList(),
			_behindByCommitCount ?? 0,
			_aheadByCommitCount ?? 0));
	}
	
	public void DispatchSetBranchAction()
	{
		var localRepo = _repo;
		if (localRepo is null)
			return;
			
		var localBranch = _branch;
		
		if (localBranch is not null)
		{
			_dispatcher.Dispatch(new GitState.SetBranchAction(
				localRepo,
				localBranch));
	
	public void DispatchSetOriginAction()
	{
		var localRepo = _repo;
			return;
			
		var localOrigin = _origin;
		
		if (localOrigin is not null)
		{
			_dispatcher.Dispatch(new GitState.SetOriginAction(
				localRepo,
				localOrigin));
		}
	}
	
	public void DispatchSetBranchListAction()
			return;
			
		var localBranchList = _branchList;
		
		if (localBranchList is not null)
			_dispatcher.Dispatch(new GitState.SetBranchListAction(
				localRepo,
				localBranchList));
    
    public List<TextEditorTextSpan> StatusParseEntire(string outputEntire)
    	_stageKind = StageKind.None;
		_repo = _gitStateWrap.Value.Repo;
		
		UntrackedGitFileList.Clear();
		StagedGitFileList.Clear();
		UnstagedGitFileList.Clear();
		_behindByCommitCount = null;
		_aheadByCommitCount = null;
    
            
		var stringWalker = new StringWalker(new ResourceUri("/__LUTHETUS__/GitCliOutputParser.txt"), outputEntire);
        
            else if (stringWalker.CurrentCharacter == ' ' && stringWalker.NextCharacter == ' ')
                // Read comments line by line
                    if (stringWalker.CurrentCharacter != ' ' || stringWalker.NextCharacter != ' ')
                        break;
                    // Discard the leading whitespace on the line (two spaces)
                    _ = stringWalker.ReadRange(2);
                    var startPositionInclusive = stringWalker.PositionIndex;
                    while (!stringWalker.IsEof && !WhitespaceFacts.LINE_ENDING_CHARACTER_LIST.Contains(stringWalker.CurrentCharacter))
                        _ = stringWalker.ReadCharacter();
                    }
                    textSpanList.Add(new TextEditorTextSpan(
                        startPositionInclusive,
                        stringWalker,
                        (byte)TerminalDecorationKind.Comment));
                }
            }
            else if (stringWalker.CurrentCharacter == WhitespaceFacts.TAB)
            {
                // Read untracked files line by line
                while (!stringWalker.IsEof)
                {
                    if (stringWalker.CurrentCharacter != WhitespaceFacts.TAB)
                        break;
                    // Discard the leading whitespace on the line (one tab)
                    _ = stringWalker.ReadCharacter();
					var gitDirtyString = string.Empty;
                    if (_stageKind == StageKind.IsReadingStagedFiles ||
                        _stageKind == StageKind.IsReadingUnstagedFiles)
                    {
                        // Read the git description
                        //
                        // Example: "new file:   BlazorApp4NetCoreDbg/Persons/Abc.cs"
                        //           ^^^^^^^^^^^^
						var gitDirtyStartPositionInclusive = stringWalker.PositionIndex;
                        while (!stringWalker.IsEof)
                        {
                            if (stringWalker.CurrentCharacter == ':')
                            {
								var gitDirtyTextSpan = new TextEditorTextSpan(
	                                gitDirtyStartPositionInclusive,
	                                stringWalker,
	                                (byte)TerminalDecorationKind.None);
								gitDirtyString = gitDirtyTextSpan.GetText();
                                // Read the ':'
                                _ = stringWalker.ReadCharacter();
                                // Read the 3 ' ' characters (space characters)
                                _ = stringWalker.ReadRange(3);
                                break;
                            _ = stringWalker.ReadCharacter();
                    }
                    var startPositionInclusive = stringWalker.PositionIndex;

                    while (!stringWalker.IsEof && !WhitespaceFacts.LINE_ENDING_CHARACTER_LIST.Contains(stringWalker.CurrentCharacter))
                    {
                        _ = stringWalker.ReadCharacter();
                    var textSpan = new TextEditorTextSpan(
                        startPositionInclusive,
                        stringWalker,
                        (byte)TerminalDecorationKind.Warning);
                    textSpanList.Add(textSpan);

                    var relativePathString = textSpan.GetText();

					var absolutePathString = PathHelper.GetAbsoluteFromAbsoluteAndRelative(
						localRepo.AbsolutePath,
                        relativePathString,
                        _environmentProvider);

                    var isDirectory = relativePathString.EndsWith(_environmentProvider.DirectorySeparatorChar) ||
                        relativePathString.EndsWith(_environmentProvider.AltDirectorySeparatorChar);

                    var absolutePath = _environmentProvider.AbsolutePathFactory(absolutePathString, isDirectory);

                    GitDirtyReason gitDirtyReason;

					if (_stageKind == StageKind.IsReadingUntrackedFiles)
					{
						gitDirtyReason = GitDirtyReason.Untracked;
					}
					else
					{
						if (gitDirtyString == "modified")
							gitDirtyReason = GitDirtyReason.Modified;
						else if (gitDirtyString == "added") // There is no "added" its "new file" in the output.
							gitDirtyReason = GitDirtyReason.Added;
						else if (gitDirtyString == "new file")
							gitDirtyReason = GitDirtyReason.Added;
						else if (gitDirtyString == "deleted")
							gitDirtyReason = GitDirtyReason.Deleted;
						else
							gitDirtyReason = GitDirtyReason.None;
					}

                    var gitFile = new GitFile(
                        absolutePath,
                        relativePathString,
                        gitDirtyReason);

                    if (_stageKind == StageKind.IsReadingUntrackedFiles)
                        UntrackedGitFileList.Add(gitFile);
                    else if (_stageKind == StageKind.IsReadingStagedFiles)
                        StagedGitFileList.Add(gitFile);
                    else if (_stageKind == StageKind.IsReadingUnstagedFiles)
                        UnstagedGitFileList.Add(gitFile);
    public List<TextEditorTextSpan> GetOriginParse(string outputEntire)
        _origin ??= outputEntire.Trim();
        
        return new();
    public List<TextEditorTextSpan> GetBranchParse(string outputEntire)
		_branch ??= outputEntire.Trim();
        return new();
    public List<TextEditorTextSpan> GetBranchListEntire(string outputEntire)
		var stringWalker = new StringWalker(new ResourceUri("/__LUTHETUS__/GitCliOutputParser.txt"), outputEntire);
		while (!stringWalker.IsEof)
		{
			// "* Abc"    <-- Line 1 with quotes added to show where it starts and ends
	        // "  master" <-- Line 2 with quotes added to show where it starts and ends
	        //
	        // Every branch seems to start with 2 characters, where the first is whether it's the active branch,
	        // and the second is just a whitespace to separate whether its the active branch from its name.
	        //
	        // Therefore, naively skip 2 characters then readline.
	        var isValid = false;
	
	        if (stringWalker.CurrentCharacter == '*' || stringWalker.CurrentCharacter == ' ')
	        {
	            if (stringWalker.NextCharacter == ' ')
	                isValid = true;
	        }
	
	        if (!isValid)
	            return textSpanList;
	
	        _ = stringWalker.ReadRange(2);
	
	        var startPositionInclusive = stringWalker.PositionIndex;
	
	        while (!stringWalker.IsEof && !WhitespaceFacts.LINE_ENDING_CHARACTER_LIST.Contains(stringWalker.CurrentCharacter))
	        {
	            _ = stringWalker.ReadCharacter();
	        }
	
	        var textSpan = new TextEditorTextSpan(
	            startPositionInclusive,
	            stringWalker,
	            (byte)TerminalDecorationKind.StringLiteral);
	            
	        textSpanList.Add(textSpan);
	        _branchList.Add(textSpan.GetText());
	        
	        if (stringWalker.IsEof)
	        {
	        	break;
	        }
	        else
	        {
	        	// Finished reading a line, so consume the line ending character(s)
	        	while (WhitespaceFacts.LINE_ENDING_CHARACTER_LIST.Contains(stringWalker.CurrentCharacter))
	        	{
		        	_ = stringWalker.ReadCharacter();
	        	}
	        }
		}