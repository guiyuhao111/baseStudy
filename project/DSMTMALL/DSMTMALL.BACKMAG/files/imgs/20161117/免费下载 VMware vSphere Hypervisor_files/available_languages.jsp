










AUI.add(
	'portal-available-languages',
	function(A) {
		var available = {};

		var direction = {};

		

			available['fr_FR'] = '?? (??)';
			direction['fr_FR'] = 'ltr';

		

			available['de_DE'] = '?? (??)';
			direction['de_DE'] = 'ltr';

		

			available['en_US'] = '?? (??)';
			direction['en_US'] = 'ltr';

		

			available['zh_CN'] = '?? (??)';
			direction['zh_CN'] = 'ltr';

		

			available['ja_JP'] = '?? (??)';
			direction['ja_JP'] = 'ltr';

		

			available['ko_KR'] = '??? (??)';
			direction['ko_KR'] = 'ltr';

		

		Liferay.Language.available = available;
		Liferay.Language.direction = direction;
	},
	'',
	{
		requires: ['liferay-language']
	}
);